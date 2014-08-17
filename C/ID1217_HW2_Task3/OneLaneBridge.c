/* 
 * File:   OneLaneBridge.c
 * Author: Kim
 * A fair implementation of one lage bridge problem
 * Commented out code represents implementation for the unfair version
 *
 * Created on February 12, 2014, 2:24 PM
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
#define NORTH_DIRECTION 1
#define SOUTH_DIRECTION -1
#define MAX_CARS 10
#define MAX_TRIPS 3
#define SHARED 0

typedef struct {
    int id, direction, trips;
} Car;
int num_cars;
sem_t no_waiting, no_accessing, counter_mutex_south, counter_mutex_north;
int num_south_travelers = 0, south_prev = 0, south_current = 0;
int num_north_travelers = 0, north_prev = 0, north_current = 0;

sem_t fifo_sem[MAX_CARS];

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

void sleep_thread(int max_sleep) {
    double start = read_timer();
    double sleep_time = (rand() % max_sleep) * 1.0e-3;
    double end;
    while (read_timer() < (end = (start + sleep_time)));
}

void change_direction(Car *c) {
    c->direction = c->direction * -1;
}

// Fifo implementation ensures that no thread is ever starved
void fifo_semaphore_wait() {
    int i;

    sem_wait(&fifo_sem[MAX_CARS - 1]);
    for (i = MAX_CARS - 2; i >= 0; i--) {
        sem_wait(&fifo_sem[i]);
        sem_post(&fifo_sem[i + 1]);
    }
}
// Posting to first index releases that thread and every other thread 
// shift one step forward in fifo queue
void fifo_semaphore_post() {
    sem_post(&fifo_sem[0]);
}

// Simulates a car
void *car(void * arg) {
    Car * c = (Car *) arg;
    printf("Car %d is up and running\n", c->id);
    if (c->id == 0)
        printf("we're 0");
    int i;
    for (i = 0; i < c->trips; i++) {
        if (c->direction == NORTH_DIRECTION) {

            //sem_wait(&no_waiting);
            fifo_semaphore_wait();
            sem_wait(&counter_mutex_north);
            north_prev = num_north_travelers;
            num_north_travelers++;
            sem_post(&counter_mutex_north);
            if (north_prev == 0)
                sem_wait(&no_accessing);
            //sem_post(&no_waiting);
            fifo_semaphore_post();

            // DRIVER OVER BRIDGE
            printf("Car %d is driving over bridge in _NORTH_ bound direction\n", c->id);
            sleep_thread(1000);

            sem_wait(&counter_mutex_north);
            num_north_travelers--;
            north_current = num_north_travelers;
            sem_post(&counter_mutex_north);
            if (north_current == 0)
                sem_post(&no_accessing);
        } else {
            //sem_wait(&no_waiting);
            fifo_semaphore_wait();
            sem_wait(&counter_mutex_south);
            south_prev = num_south_travelers;
            num_south_travelers++;
            sem_post(&counter_mutex_south);
            if (south_prev == 0)
                sem_wait(&no_accessing);
            //sem_post(&no_waiting);
            fifo_semaphore_post();

            // DRIVE OVER BRIDGE
            printf("Car %d is driving over bridge in _SOUTH_ bound direction\n", c->id);
            sleep_thread(1000);

            sem_wait(&counter_mutex_south);
            num_south_travelers--;
            south_current = num_south_travelers;
            sem_post(&counter_mutex_south);
            if (south_current == 0)
                sem_post(&no_accessing);
        }
        sleep_thread(10000);
        change_direction(c);
    }
    printf("Car %d is done traveling\n", c->id);
    free(c);

}

int main(int argc, char** argv) {
    int num_trips =  (argc > 1) ? atoi(argv[1]) : MAX_TRIPS;
    num_cars = MAX_CARS;
    pthread_attr_t attr;
    pthread_attr_init(&attr);
    pthread_attr_setscope(&attr, PTHREAD_SCOPE_SYSTEM);

    sem_init(&counter_mutex_south, SHARED, 1);
    sem_init(&counter_mutex_north, SHARED, 1);
    sem_init(&no_accessing, SHARED, 1);
    //sem_init(&no_waiting, SHARED, 1);

    // Init sem arr for fifo semaphore function
    int i;
    for (i = 0; i < MAX_CARS; i++) {
        sem_init(&fifo_sem[i], SHARED, 1);

    }

    pthread_t cars[num_cars];
    Car *c;
    for (i = 0; i < num_cars; i++) {
        c = malloc(sizeof (Car));
        c->id = i;
        c->direction = i % 2 == 0 ? SOUTH_DIRECTION : NORTH_DIRECTION;
        c->trips = num_trips;
        pthread_create(&cars[i], &attr, car, (void *) c);
        printf("Created a car with id %d direction %d and number of trips %d\n", c->id, c->direction, c->trips);
    }

    for (i = 0; i < num_cars; i++)
        pthread_join(cars[i], NULL);

    return (EXIT_SUCCESS);
}

