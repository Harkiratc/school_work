/* 
 * File:   main.c
 * Author: Kim
 * Fairness:
 * The solution in not guaranteed fair because semaphores in pthread are not
 * guaranteed fair. It is platform dependent whether post wakes up threads
 * in fifo order or in some other (random) order, thus we can't guarantee
 * on all platforms
 * Created on February 10, 2014, 8:19 PM
 */
#ifndef _REENTRANT
#define _REENTRANT
#endif
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <semaphore.h>
#include <stdbool.h>
#include <time.h>
#include <sys/time.h>
#include <unistd.h>
#define FULL_BOWL 10
#define SHARED 0
#define MAX_CHILDREN 10

void *parent_bird(void *);
void *child_bird(void *);


sem_t empty_bowl, non_empty_bowl, mutex_bowl;
int num_items_in_bowl = 0, size = 0, full_bowl = 0;


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

void sleep_thread() {
    double start = read_timer();
    double sleep_time = (rand() % 9999) * 1.0e-3;
    double end;
    while(read_timer() < (end = (start + sleep_time)));
    printf("Slept for %f\n", (end - start));
}

// Simulates a child bird, repeatedly eating from bowl one at a time
// when it notices the bowl is empty it signals the parent
void *child_bird(void *arg) {
    printf("child started\n");
    while (true) {
        sem_wait(&non_empty_bowl);
        num_items_in_bowl--;
        printf("Child %d has eaten, %d remaining in bowl\n", *((int *)arg), num_items_in_bowl);
        if (num_items_in_bowl == 0) {
            printf("Bowl has %d item signaling parent to refill\n", num_items_in_bowl);
            sem_post(&empty_bowl);
        } else
            sem_post(&non_empty_bowl);

        sleep_thread();
    }

}
// Refills the bowl when bowl is empty
void *parent_bird(void *arg) {
    printf("parent started\n");
    while (true) {
        sem_wait(&empty_bowl);
        printf("Parent filling bowl, bowl had %d items now\n", num_items_in_bowl);
        num_items_in_bowl += full_bowl;
        sem_post(&non_empty_bowl);
    }

}

int main(int argc, char** argv) {
    size =  (argc > 1) ? atoi(argv[1]) : MAX_CHILDREN;
    full_bowl =  (argc > 2) ? atoi(argv[2]) : FULL_BOWL;
    /* thread ids and attributes */
    pthread_t parent;
    pthread_t children[MAX_CHILDREN];
    pthread_attr_t attr;
    pthread_attr_init(&attr);
    pthread_attr_setscope(&attr, PTHREAD_SCOPE_SYSTEM);

    sem_init(&mutex_bowl, SHARED, 1);
    sem_init(&empty_bowl, SHARED, 1);
    sem_init(&non_empty_bowl, SHARED, 0);

    pthread_create(&parent, &attr, parent_bird, NULL);
    int i;
    int *my_id;
    for (i = 0; i < size; i++)
    {
        my_id = malloc(sizeof(int));
        (*my_id) = i;
        pthread_create(&children[i], &attr, child_bird, (void *) my_id);
    }
    pthread_join(parent, NULL);


    return (EXIT_SUCCESS);
}

