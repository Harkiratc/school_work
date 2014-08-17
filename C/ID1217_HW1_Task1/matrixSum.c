/* matrix summation using pthreads

   features: uses a barrier; the Worker[0] computes
             the total sum from partial sums computed by Workers
             and prints the total sum to the standard output

   usage under Linux:
     gcc matrixSum.c -lpthread
     a.out size numWorkers

 */
#ifndef _REENTRANT 
#define _REENTRANT 
#endif 
#include <pthread.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdbool.h>
#include <time.h>
#include <sys/time.h>
#include <limits.h>
#define MAXSIZE 10000  /* maximum matrix size */
#define MAXWORKERS 10   /* maximum number of workers */

pthread_mutex_t barrier; /* mutex lock for the barrier */
pthread_cond_t go; /* condition variable for leaving */
int numWorkers; /* number of workers */
int numArrived = 0; /* number who have arrived */


int row_count = 0;

/* Struct for storing thread max values*/
typedef struct {
    int min, min_row, min_col, max, max_row, max_col, sum;
} ThreadData;

// Fetches and increments a counter with mutual exclusion
int fetch_and_inc() {
    int ret;
    pthread_mutex_lock(&barrier);
    ret = row_count;
    row_count++;
    pthread_mutex_unlock(&barrier);
    return ret;
}

/* a reusable counter barrier */
void Barrier() {
    pthread_mutex_lock(&barrier);
    numArrived++;
    if (numArrived == numWorkers) {
        numArrived = 0;
        pthread_cond_broadcast(&go);
    } else
        pthread_cond_wait(&go, &barrier);
    pthread_mutex_unlock(&barrier);
}

/* timer */
double read_timer() {
    static bool initialized = false;
    static struct timeval start;
    struct timeval end;
    if (!initialized) {
        gettimeofday(&start, NULL);
        initialized = true;
    }
    gettimeofday(&end, NULL);
    return (end.tv_sec - start.tv_sec) + 1.0e-6 * (end.tv_usec - start.tv_usec);
}

double start_time, end_time; /* start and end times */
int size, stripSize; /* assume size is multiple of numWorkers */
//int sums[MAXWORKERS]; /* partial sums */
int matrix[MAXSIZE][MAXSIZE]; /* matrix */
//ThreadMinMax threads_min_max[MAXWORKERS]; /* every thread calculates its own max local max*/
//bool threads_done_calc[MAXWORKERS];

void *Worker(void *);

/* read command line, initialize, and create threads */
int main(int argc, char *argv[]) {
    int i, j;
    long l; /* use long in case of a 64-bit system */
    pthread_attr_t attr;
    pthread_t workerid[MAXWORKERS];

    /* set global thread attributes */
    pthread_attr_init(&attr);
    pthread_attr_setscope(&attr, PTHREAD_SCOPE_SYSTEM);

    /* initialize mutex and condition variable */
    pthread_mutex_init(&barrier, NULL);
    pthread_cond_init(&go, NULL);

    /* read command line args if any */
    size = (argc > 1) ? atoi(argv[1]) : MAXSIZE;
    numWorkers = (argc > 2) ? atoi(argv[2]) : MAXWORKERS;
    if (size > MAXSIZE) size = MAXSIZE;
    if (numWorkers > MAXWORKERS) numWorkers = MAXWORKERS;
    stripSize = size / numWorkers;

    /* initialize the matrix */
    for (i = 0; i < size; i++) {
        for (j = 0; j < size; j++) {
            matrix[i][j] = rand() % 99;
        }
    }


    /* do the parallel work: create the workers */
    start_time = read_timer();
    for (l = 0; l < numWorkers; l++)
        pthread_create(&workerid[l], &attr, Worker, (void *) l);

    ThreadData *ret_val;
    ThreadData total_min_max = {INT_MAX, -1, -1, INT_MIN, -1, -1, 0};
    for (l = 0; l < numWorkers; l++) {
        pthread_join(workerid[l], (void **) &ret_val);
#ifdef DEBUG
        printf("T: %d has finished\n", (int) l);
#endif
        if (ret_val->max > total_min_max.max) {
            total_min_max.max = ret_val->max;
            total_min_max.max_col = ret_val->max_col;
            total_min_max.max_row = ret_val->max_row;
        }
        if (ret_val->min < total_min_max.min) {
            total_min_max.min = ret_val->min;
            total_min_max.min_col = ret_val->min_col;
            total_min_max.min_row = ret_val->min_row;
        }
        total_min_max.sum += ret_val->sum;
        free(ret_val);

    }
    end_time = read_timer();
    /* print results */
    printf("The total is %d\n", total_min_max.sum);
    printf("The max value is %d on matrix[%d][%d]\n", total_min_max.max, total_min_max.max_row, total_min_max.max_col);
    printf("The min value is %d on matrix[%d][%d]\n", total_min_max.min, total_min_max.min_row, total_min_max.min_col);
            

#ifdef DEBUG
    printf("Max: matrix[%d][%d] = %d\nMin: matrix[%d][%d] = %d\n", total_min_max.max_row, total_min_max.max_col,
            matrix[total_min_max.max_row][total_min_max.max_col], total_min_max.min_row, total_min_max.min_col,
            matrix[total_min_max.min_row][total_min_max.min_col]);
#endif

    printf("The execution time is %g sec\n", end_time - start_time);
    //pthread_exit(NULL);
}

// Each worker computes the, sum, min & max values for tasks from taken from bag
// while there are still tasks left to be computed
void *Worker(void *arg) {
    long myid = (long) arg;
    int total, i, j, first, last;
    ThreadData *my_min_max = malloc(sizeof (ThreadData));
    my_min_max-> max = INT_MIN;
    my_min_max-> min = INT_MAX;
    my_min_max->sum = 0;
    my_min_max->max_col = -1;
    my_min_max->max_row = -1;
    my_min_max->min_col = -1;
    my_min_max->min_row = -1;

#ifdef DEBUG
    // printf("worker %d (pthread id %d) has started\n", myid, pthread_self());
#endif


    /* sum values in my strip */
    total = 0;
#ifdef DEBUG
    printf("T: %d started\n", (int) myid);
#endif
    int my_row;
    while ((my_row = fetch_and_inc()) < size) {
#ifdef DEBUG
        printf("T: %d started with row %d\n", (int) myid, my_row);
#endif
        for (j = 0; j < size; j++) {
            total += matrix[my_row][j];
            if (matrix[my_row][j] > my_min_max->max) {
                my_min_max->max = matrix[my_row][j];
                my_min_max->max_row = my_row;
                my_min_max->max_col = j;
            }
            if (matrix[my_row][j] < my_min_max->min) {
                my_min_max->min = matrix[my_row][j];
                my_min_max->min_row = my_row;
                my_min_max->min_col = j;

            }
            my_min_max->sum = total;
        }
    }

#ifdef DEBUG
    printf("T: %d done with total %d\n", (int) myid, my_min_max->sum);
#endif
    pthread_exit(my_min_max);

}