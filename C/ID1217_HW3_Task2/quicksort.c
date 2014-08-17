/* 
 * File:   main.c
 * Author: kikko
 *
 * Created on February 21, 2014, 4:13 PM
 * Parallel version of quicksort that uses OpenMP
 * @Author Kim Persson
 */

#include <stdio.h>
#include <stdlib.h>
#include <omp.h>
int size, numWorkers;
#define MAXSIZE 100000000
#define MAXWORKERS 8
int array[MAXSIZE];


typedef struct {
    int start, end;
} range;

void randomize_arr() {
    /* initialize the matrix */
    int i;
    for (i = 0; i < size; i++) {
        array[i] = rand() % 9999999;
    }
}

void swap(int inarr[], int i, int j) {
    int temp = inarr[i];
    inarr[i] = inarr[j];
    inarr[j] = temp;
}

int partition(int inarr[], int lo, int hi) {
    int b = lo;
    int r = (int) (lo + (hi - lo + 1)*(1.0 * rand() / RAND_MAX));
    int pivot = inarr[r];
    swap(inarr, r, hi);
    int i;
    for (i = lo; i < hi; i++) {
        if (inarr[i] < pivot) {
            swap(inarr, i, b);
            b++;
        }
    }
    swap(inarr, hi, b);
    return b;
}
void sequential_quicksort(int *in_arr, int start, int end) {
    if (start >= end)
        return;

    int pivot_pos = partition(in_arr, start, end);
    sequential_quicksort(in_arr, start, pivot_pos - 1);
    sequential_quicksort(in_arr, pivot_pos + 1, end);
}
// Parallel quicksort creates tasks for the for doing sorting in parallel 
// After reaching a certain recursive depth rest of sorting is done 
// sequentially to minimize overhead
void quicksort(int *in_arr, int start, int end, int depth) {
    if(start < end)
    {
        if(depth)
        {
            int pivot = partition(in_arr,start,end);
            #pragma omp task
            quicksort(in_arr,start, pivot - 1,depth - 1);
            #pragma omp task
            quicksort(in_arr,pivot + 1,  end,depth - 1);
            
        } else {
            int pivot = partition(in_arr,start,end);
            sequential_quicksort(in_arr,start, pivot - 1);
            sequential_quicksort(in_arr,pivot + 1,  end);
        }
    }

    

}
// Starts multi threaded quicksort
void do_quicksort(int *ar, int size)
{
    #pragma omp parallel
    {
        #pragma omp single nowait
        {
            quicksort(ar, 0, size -1, 15);
        }
    }
   
}

void print_arr() {
    int i;
    for (i = 0; i < size; i++) {
        printf("[");
        printf("%d", array[i]);
        printf("]");
    }
    printf("\n");
}

void check_sorting() {
    int i;
    for (i = 0; i < size - 1; i++) {
        if (array[i] > array[i + 1]) {
            printf("Array not sorted correctly at cols %d, %d\n", i, (i + 1));
            return;
        }
    }
    printf("Array sorted correctly\n");
}


int main(int argc, char** argv) {
    size = (argc > 1) ? atoi(argv[1]) : MAXSIZE;
    numWorkers = (argc > 2) ? atoi(argv[2]) : MAXWORKERS;
    if (size > MAXSIZE) size = MAXSIZE;
    if (numWorkers > MAXWORKERS) numWorkers = MAXWORKERS;
    omp_set_num_threads(numWorkers);
    int t = omp_get_max_threads();
    printf("threads %d\n", t);
    randomize_arr();
    double start_time = omp_get_wtime();
    do_quicksort(array,size);
    double end_time = omp_get_wtime();
    printf("Time taken %f\n", (end_time - start_time));
    check_sorting();
    return (EXIT_SUCCESS);
}

