/* 
 * File:   Tee.c
 * Author: Kim Persson
 * A multithreaded version of the unix command tee
 * main thread reads input from stdin
 * 1 thread writes read input to stdout
 * 1 thread writes read input to a file
 * takes filename as argument
 */

#ifndef _REENTRANT 
#define _REENTRANT 
#endif 
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <time.h>
#include <sys/time.h>
#include <stdbool.h>
#include <string.h>
#define READ_BUFFER_SIZE 80
pthread_attr_t attr;
pthread_mutex_t stdout_lock, fileout_lock;



typedef struct Node Node;

struct Node {
    Node *next;
    char *data;

};
Node *stdout_tail, *stdout_head;
Node *fileout_tail, *fileout_head;

// This does not require locks even though it is a shared variable
// it is only written to when reader is done, worst case
// the writers can read any value, true or false when a read overlaps
// a write, both of which are fine
bool finished_reading = false;

// Prints to stdout and frees resources from node
void print_and_free_node(Node *node) {
    if(node->data != NULL)
        printf("%s", node->data);
    free(node->data);
    free(node);
}

// Head of list needs to be accessed with mutual exclusion because
// reader adds elements by changing head's next pointer
bool at_head(Node * node, Node * head_to_compare,pthread_mutex_t *lock) {
    bool is_head;
    pthread_mutex_lock(lock);
    is_head = node == head_to_compare;
    pthread_mutex_unlock(lock);
    return is_head;

}
// Writes elements added to its list to stdout and frees resources
void *write_stdout() {
    Node *curr = stdout_tail;
    Node *next;
    while (true) {
        if (!at_head(curr,stdout_head, &stdout_lock)) {
            next = curr->next;
            print_and_free_node(curr);
            curr = next;
        } else if (finished_reading) {
            print_and_free_node(curr);
            break;
        }
    }
}
// Writes to file and frees node resources
void write_and_free_node(Node *node, FILE *fp) {
    if(node->data != NULL) {
        fputs(node->data,fp);
    }
    free(node->data);
    free(node);
}
// Writes elements added to its list to file of specified name and frees
// resources
void *write_fileout(void *void_filename) {
    char * filename = (char *)void_filename;
    Node *curr = fileout_tail;
    Node *next;
    
    FILE *fp = fopen(filename,"w+");
    while (true) {
        if (!at_head(curr, fileout_head ,&fileout_lock)) {

            next = curr->next;
            write_and_free_node(curr,fp);
            curr = next;
        } else if (finished_reading) {
            write_and_free_node(curr,fp);
            break;
        }
    }
    fclose(fp);
}

// Multithreaded tee command, main thread reads from stdin
// one thread writes to stdout and one thread writes to a specified file
// each writer has its own linked list on which the reader adds all read
// information. This way no synchronization is needed between the two writers
// and minimal synchronization is needed between reader and writer
void tee(char filename[]) {
    // initialize pthread
    pthread_attr_init(&attr);
    void * a;
    pthread_attr_setscope(&attr, PTHREAD_SCOPE_SYSTEM);
    pthread_mutex_init(&stdout_lock, NULL);
    pthread_mutex_init(&fileout_lock, NULL);
    
    // Initiate lists
    stdout_tail = malloc(sizeof (Node));
    stdout_head = stdout_tail;
    fileout_tail = malloc(sizeof (Node));
    fileout_head = fileout_tail;     
   
    pthread_t w_stdout,w_fileout;
    pthread_create(&w_stdout, &attr, write_stdout, a);
    pthread_create(&w_fileout,&attr,write_fileout,(void *) filename);
    int i;
    bool done = false;
    Node *node;
    char *buf;
    while (!done) {
        buf = malloc(sizeof (char) * READ_BUFFER_SIZE);
        for (i = 0; i < sizeof (buf) - 1; i++) {
            int c = getchar();
            if (c == EOF) {
                buf[i] = '\0';
                done = true;
                break;
            } 
            buf[i] = c;
        }
        buf[sizeof (buf) - 1] = '\0';
        node = malloc(sizeof (Node));
        node->next = NULL;
        node->data = buf;

        pthread_mutex_lock(&stdout_lock);
        stdout_head->next = node;
        stdout_head = node;
        pthread_mutex_unlock(&stdout_lock);
        
        node = malloc(sizeof (Node));
        node->next = NULL;
        node->data = malloc(sizeof buf);
        memcpy(node->data,buf,sizeof(buf));
        pthread_mutex_lock(&fileout_lock);
        fileout_head->next = node;
        fileout_head = node;
        pthread_mutex_unlock(&fileout_lock);
        
        
    }
    finished_reading = true;
    pthread_join(w_stdout, NULL);
    pthread_join(w_fileout,NULL);
}

int main(int argc, char** argv) {
    char* filename;
    if (argc > 1)
        filename = argv[1];
    else
        filename = "tee.out";
#ifdef DEBUG
    printf("Output file: %s\n", filename);
#endif
    tee(filename);
}