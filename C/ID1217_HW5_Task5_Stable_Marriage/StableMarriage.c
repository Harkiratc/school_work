/* 
 * File:   main.c
 * Author: Kim Persson
 * Solves a distributed version of the marriage problem
 * Created on March 4, 2014, 10:35 PM
 */

#include <stdio.h>
#include <stdlib.h>
#include <mpi.h>
#include <stdbool.h>
#include <string.h>
#define PROPOSAL 10
#define FINISHED 100
const int ACCEPTED_PROPOSAL = -1;
const int REJECTED_PROPOSAL = -2;
const int EXECUTION_DONE = -3;

/*
 * 
 */
void swap(int *list, int a, int b) {
    int tmp = list[a];
    list[a] = list[b];
    list[b] = tmp;
}
// Creates a randomized ranking of the persons of the opposite sex
void create_ranking(int * list, int ranking_size, int rank) {
    int i;
    int start = rank % 2 == 0 ? 1 : 0;
    for (i = 0; i < ranking_size; i++) {
        list[i] = start + i * 2;
    }

    srand(time(NULL) * rank);
    for (i = 0; i < ranking_size; i++)
        swap(list, i, (rand() % ranking_size));
}

void print_list(int *list, int size) {
    int i;
    for (i = 0; i < size; i++)
        printf("%d ", list[i]);
    printf("\n");

}

// Creates a string with rankings in list for printout
char* list_string(int *list, int size) {
     int i;
    char buf[100];
    char *buf2 = (char *)malloc(sizeof(char) * 1024);
    int pos = 0;
    for (i = 0; i < size; i++) {
        snprintf(buf,10,"%d ",list[i]);
        memcpy(&buf2[pos], &buf, strlen(buf));
        pos += strlen(buf);
    }
    return buf2;
}

// Finds out the rank of a specific id
int ranking_position(int *list, int ranking_size, int id) {
    int i;
    
    for (i = 0; i < ranking_size; i++) {
        if (list[i] == id)
            return i;
    }
//     This should never happen
    return -10000;
}

int main(int argc, char** argv) {
    int rank;
    int size;

    MPI_Init(0, 0);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);
    MPI_Status status;
    int ranking_size = ((size - 1) / 2);
    int ranking[ranking_size];
    int *response = malloc(sizeof (int));

    // We need an even amount of men and women to produce matching of everyone
    // but we also need a process tells everyone when stable matching is reached which in this case 
    // is the process with the highest id
    if (size % 2 != 1) {
        printf("ERROR invalid number of processes specified\nUsage: please specify an odd number of processes to allow matching between men and women and an additional engagement supervisor process\nExiting...\n");
        MPI_Finalize();
        return -1;
    }


    // The last process is responsible for keeping keeping count of marriages 
    // if all men are married this means that there's no on left to propose
    // that means algorithm can terminate safely
    if (rank == size - 1) {
        printf("%d is responsible for reviewing marriages\n", rank);
        // While we don't have all men engaged the algorithm continues
        int engaged_men = 0;
        while (engaged_men < ((size - 1) / 2)) {
            MPI_Recv((void *) response, 1, MPI_INT, MPI_ANY_SOURCE, MPI_ANY_TAG, MPI_COMM_WORLD, &status);
            if (*response == 1)
                engaged_men++;
            else if (*response == -1)
                engaged_men--;
            else
                printf("ERROR IN ENGAGEMENT COUNT, RECEIVED INVALID MESSAGE\n");

        }
        // Notify everyone
        // Can't use broadcast because they are expecting regular message
        int i;
        for (i = 0; i < size - 1; i++) {
            MPI_Send((void *) &EXECUTION_DONE, 1, MPI_INT, i, PROPOSAL, MPI_COMM_WORLD);
        }

    } // We are a man
    else if (rank % 2 == 1) {
        create_ranking(ranking,ranking_size, rank);
        char *list_str = list_string(ranking,ranking_size);
        printf("%d's list : %s\n", rank, list_str);
        int i, notify;
        for (i = 0; i < ranking_size; i++) {
            // Propose to women in the oder of our list
            MPI_Send((void *) &rank, 1, MPI_INT, ranking[i], PROPOSAL, MPI_COMM_WORLD);
            printf("%d sent proposal to %d\n", rank, ranking[i]);
            MPI_Recv((void *) response, 1, MPI_INT, MPI_ANY_SOURCE, MPI_ANY_TAG, MPI_COMM_WORLD, &status);
            printf("%d received response %d from %d (-1 = ACCEPTED proposal & -2 REJECTED proposal)\n", rank, *response, ranking[i]);
            // If we get an accept we are engaged and don't have to do anything until we get dumped
            // or application is finished
            if (*response == ACCEPTED_PROPOSAL) {
                printf("%d is now engaged to %d\n", rank, ranking[i]);
                notify = 1;
                MPI_Send((void *) &notify, 1, MPI_INT, (size - 1), PROPOSAL, MPI_COMM_WORLD);
                MPI_Recv((void *) response, 1, MPI_INT, MPI_ANY_SOURCE, MPI_ANY_TAG, MPI_COMM_WORLD, &status);
                if (*response == REJECTED_PROPOSAL) {
                    printf("%d's engagement broken off with %d\n", rank, ranking[i]);
                    notify = -1;
                    MPI_Send((void *) &notify, 1, MPI_INT, (size - 1), PROPOSAL, MPI_COMM_WORLD);
                    continue;
                } else if (*response == EXECUTION_DONE) {
                    printf("Done %d matched with %d\n", rank, ranking[i]);
                    break;
                }
            }


        }
    } else {
        create_ranking(ranking,ranking_size, rank);
        printf("%d's list :\n", rank);
        print_list(ranking, ranking_size);
        int current_partner = -1;
        int old_partner;
        while (true) {
            MPI_Recv((void *) response, 1, MPI_INT, MPI_ANY_SOURCE, MPI_ANY_TAG, MPI_COMM_WORLD, &status);
            // We are not done, it must be a proposal
            if (*response != EXECUTION_DONE) {
                // We have no partner, must accept proposal
                if (current_partner == -1) {
                    current_partner = status.MPI_SOURCE;
                    MPI_Send((void *) &ACCEPTED_PROPOSAL, 1, MPI_INT, current_partner, PROPOSAL, MPI_COMM_WORLD);
                    printf("%d accepted %d's proposal\n", rank, current_partner);
                }// We like new proposer better
                else if (ranking_position(ranking, ranking_size, status.MPI_SOURCE) < ranking_position(ranking, ranking_size, current_partner)) {
                    printf("%d ranks new proposer %d, and current partner at %d\n", rank, ranking_position(ranking, ranking_size, status.MPI_SOURCE), ranking_position(ranking, ranking_size, current_partner));
                    // Break off our current engagement
                    MPI_Send((void *) &REJECTED_PROPOSAL, 1, MPI_INT, current_partner, PROPOSAL, MPI_COMM_WORLD);
                    old_partner = current_partner;

                    // Accept new proposer
                    current_partner = status.MPI_SOURCE;
                    MPI_Send((void *) &ACCEPTED_PROPOSAL, 1, MPI_INT, current_partner, PROPOSAL, MPI_COMM_WORLD);
                    printf("%d broke it off with partner %d and accepted %d's proposal instead\n", rank, old_partner, current_partner);
                }// We like our current partner better
                else {
                    printf("%d ranks new proposer %d and current partner %d\n", rank, ranking_position(ranking, ranking_size, status.MPI_SOURCE), ranking_position(ranking, ranking_size, current_partner));
                    MPI_Send((void *) &REJECTED_PROPOSAL, 1, MPI_INT, status.MPI_SOURCE, PROPOSAL, MPI_COMM_WORLD);
                }

            } else if (*response == EXECUTION_DONE) {
                break;
            }
        }
    }
    MPI_Finalize();
    return 0;
}

