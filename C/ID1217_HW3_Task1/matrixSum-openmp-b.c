
/* matrix summation using OpenMP

   usage with gcc (version 4.2 or higher required):
     gcc -O -fopenmp -o matrixSum-openmp matrixSum-openmp.c 
     ./matrixSum-openmp size numWorkers

 */

#include <omp.h>
#include <limits.h>

double start_time, end_time;

#include <stdio.h>
#define MAXSIZE 20000  /* maximum matrix size */
#define MAXWORKERS 8   /* maximum number of workers */

int numWorkers;
int size;
int matrix[MAXSIZE][MAXSIZE];
void *Worker(void *);

typedef struct {
    int max, min, max_row, max_col, min_row, min_col;
} Data;

Data common_values[MAXSIZE];

/* read command line, initialize, and create threads */
int main(int argc, char *argv[]) {
    int i, j;
    long total = 0;

    /* read command line args if any */
    size = (argc > 1) ? atoi(argv[1]) : MAXSIZE;
    numWorkers = (argc > 2) ? atoi(argv[2]) : MAXWORKERS;
    if (size > MAXSIZE) size = MAXSIZE;
    if (numWorkers > MAXWORKERS) numWorkers = MAXWORKERS;

    omp_set_num_threads(numWorkers);

    /* initialize the matrix */
    for (i = 0; i < size; i++) {
//          printf("[ ");
        for (j = 0; j < size; j++) {
            matrix[i][j] = rand() % 99;
//            	  printf(" %d", matrix[i][j]);
        }
//        	  printf(" ]\n");
    }



    // Each thread works with it's own private struct to minimize synchronization
    // when has calculated the values for its portion of the matrix it enters a 
    // critical section and updates the value if it is better than 
    // the value already set
    Data own_values = {.max = INT_MIN, .min = INT_MAX, .max_row = -1, .max_col = -1, .min_row = -1, .min_col = -1};
    Data results = {.max = INT_MIN, .min = INT_MAX, .max_row = -1, .max_col = -1, .min_row = -1, .min_col = -1};
    start_time = omp_get_wtime();
#pragma omp parallel for reduction (+:total) private(j) firstprivate(own_values) shared(results)
    for (i = 0; i < size; i++) {
        for (j = 0; j < size; j++) {
            total += matrix[i][j];
            if (matrix[i][j] > own_values.max) {
                own_values.max = matrix[i][j];
                own_values.max_row = i;
                own_values.max_col = j;
            }
            if (matrix[i][j] < own_values.min) {
                own_values.min = matrix[i][j];
                own_values.min_row = i;
                own_values.min_col = j;
            }
        }
        #pragma omp critical 
        {
            if(own_values.max > results.max)
            {
                results.max = own_values.max;
                results.max_row = own_values.max_row;
                results.max_col = own_values.max_col;
            }
            if(own_values.min < results.min)
            {
                results.min = own_values.min;
                results.min_row = own_values.min_row;
                results.min_col =  own_values.min_col;
            }
        }
    }
    // implicit barrier
    end_time = omp_get_wtime();
    printf("the total is %ld\n", total);
    printf("max is %d at matrix[%d][%d] its value is %d\n", results.max, results.max_row, results.max_col, matrix[results.max_row][results.max_col]);
    printf("min is %d at matrix[%d][%d] its value is %d\n", results.min, results.min_row, results.min_col, matrix[results.min_row][results.min_col]);
    printf("it took %g seconds\n", end_time - start_time);

}
