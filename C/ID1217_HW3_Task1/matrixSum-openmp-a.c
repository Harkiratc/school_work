
/* matrix summation using OpenMP

   usage with gcc (version 4.2 or higher required):
     gcc -O -fopenmp -o matrixSum-openmp matrixSum-openmp.c 
     ./matrixSum-openmp size numWorkers

 */

#include <omp.h>
#include <limits.h>

double start_time, end_time;

#include <stdio.h>
#define MAXSIZE 10000  /* maximum matrix size */
#define MAXWORKERS 4   /* maximum number of workers */

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
    int i, j, total = 0;

    /* read command line args if any */
    size = (argc > 1) ? atoi(argv[1]) : MAXSIZE;
    numWorkers = (argc > 2) ? atoi(argv[2]) : MAXWORKERS;
    if (size > MAXSIZE) size = MAXSIZE;
    if (numWorkers > MAXWORKERS) numWorkers = MAXWORKERS;

    omp_set_num_threads(numWorkers);

    /* initialize the matrix */
    for (i = 0; i < size; i++) {
        //  printf("[ ");
        for (j = 0; j < size; j++) {
            matrix[i][j] = rand() % 99;
            //	  printf(" %d", matrix[i][j]);
        }
        //	  printf(" ]\n");
    }

    start_time = omp_get_wtime();



    Data own_values = {.max = INT_MIN, .min = INT_MAX, .max_row = -1, .max_col = -1, .min_row = -1, .min_col = -1};
#pragma omp parallel for reduction (+:total) private(j) firstprivate(own_values)
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
        common_values[i] = own_values;
    }

    // implicit barrier
    int max = INT_MIN, min = INT_MAX, max_row = -1, max_col = -1, min_row = -1, min_col = -1;
    for (i = 0; i < size; i++) {
        if (common_values[i].max > max) {
            max = common_values[i].max;
            max_row = common_values[i].max_row;
            max_col = common_values[i].max_col;
        }
        if (common_values[i].min < min) {
            min = common_values[i].min;
            min_row = common_values[i].min_row;
            min_col = common_values[i].min_col;

        }
    }
    end_time = omp_get_wtime();


    printf("the total is %d\n", total);
    printf("max is %d at matrix[%d][%d] its value is %d\n", max, max_row, max_col, matrix[max_row][max_col]);
    printf("min is %d at matrix[%d][%d] its value is %d\n", min, min_row, min_col, matrix[min_row][min_col]);
    printf("it took %g seconds\n", end_time - start_time);

}
