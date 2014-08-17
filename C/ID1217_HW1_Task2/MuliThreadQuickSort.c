/* 
 * File:   main.c
 * Author: Kim Persson
 * Sorts a randomized array using a multithreaded version of quicksort 
 * Created on February 5, 2014, 3:46 PM
 */
#ifndef _REENTRANT 
#define _REENTRANT 
#endif 
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <time.h>
#include <sys/time.h>
#include <limits.h>
#include <stdbool.h>
#include <string.h>
#define MAX_SIZE 10000000
// Ranges lower than this are sorted sequentially because overhead is too for
// creating a thread and starting it for such small list
#define MIN_ARR_SIZE 10000
int array[MAX_SIZE];
int size;
int num_threads = 0;
pthread_attr_t attr;

/*
 * 
 */

typedef struct {
    int start, end;
} range;

void randomize_arr() {
    /* initialize the matrix */
    int i;
    for (i = 0; i < size; i++) {
        array[i] = rand() % 99999;
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

// Normal version of quicksort, used for sorting small ranges where overhead
// of creating a thread and sorting is much larger than just sorting sequentially
int* sequential_quicksort(int *in_arr, int start, int end) {
    if (start >= end)
        return in_arr;

    int pivot_pos = partition(in_arr, start, end);
    sequential_quicksort(in_arr, start, pivot_pos - 1);
    sequential_quicksort(in_arr, pivot_pos + 1, end);
    return in_arr;
}
// For benchmark purposes, makes a clean copy of array to sort 
// before calling sequential quicksort
void do_seq_quicksort() {
    int *arr_cpy = malloc(sizeof (array));
    memcpy(arr_cpy, array, sizeof (array));
    double start = read_timer();
    sequential_quicksort(arr_cpy, 0, size);
    double end = read_timer();
    printf("Sequential quicksort time: %f\n", (end - start));
    free(arr_cpy);
}

// multi threaded version of quicksort, recursivly spawns new threads
// for ranges lower and higher than pivot until range is small enough
// to sort sequentially 
void *quicksort(void *arg) {
    range *r = (range*) arg;
    // Base case for recursion
    if (r->start >= r->end)
        pthread_exit(NULL);

    int pivot_pos = partition(array, r->start, r->end);
    range r1 = {r->start, pivot_pos - 1};
    range r2 = {pivot_pos + 1, r->end};
    // If our range is large enough create new threads otherwise do sequentially
    if ((r->end - r->start) > MIN_ARR_SIZE) {

        /* set global thread attributes */
        pthread_t w1, w2;
        pthread_create(&w1, &attr, quicksort, (void *) &r1);
        pthread_create(&w2, &attr, quicksort, (void *) &r2);
        pthread_join(w1, NULL);
        pthread_join(w2, NULL);
    } else {
        sequential_quicksort(array, r1.start, r1.end);
        sequential_quicksort(array, r2.start, r2.end);
    }

    pthread_exit(NULL);
}
// Starts multithreaded quicksort
void do_parallel_quicksort() {
    range array_range = {0, size - 1};
    /* set global thread attributes */
    pthread_t worker;
    pthread_attr_init(&attr);
    pthread_attr_setscope(&attr, PTHREAD_SCOPE_SYSTEM);
    double start = read_timer();
    pthread_create(&worker, &attr, quicksort, (void *) &array_range);
    pthread_attr_destroy(&attr);
    pthread_join(worker, NULL);
    double end = read_timer();
    printf("Time: %f\n", (end - start));



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
// Checks if array is sorted correctly
void check_sorting() {
    int i;
    for (i = 0; i < size - 1; i++) {
        if (array[i] > array[i + 1]) {
            printf("Array not sorted correctly at cols %d, %d\n",i, (i+1));
            return;
        }
    }
    printf("Array sorted correctly\n");
}

int main(int argc, char** argv) {
    /* read command line args if any */
    size = (argc > 1) ? atoi(argv[1]) : MAX_SIZE;
    if (size > MAX_SIZE) size = MAX_SIZE;
    randomize_arr();
#ifdef DEBUG
    print_arr();
#endif
    do_seq_quicksort();
    do_parallel_quicksort();
    check_sorting();
#ifdef DEBUG
    print_arr();
#endif
    return (EXIT_SUCCESS);
}

