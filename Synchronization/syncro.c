#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <stdbool.h>

int volatile mutex = 0;
int volatile c;

void enter_critical_section()
{
    __asm volatile("pusha");
    __asm volatile("1:");
    __asm volatile("mov %0, %%ebx" : : "r" (&mutex));
    __asm volatile("movl $0, %eax");
    __asm volatile("movl $1, %edx");
    __asm volatile("lock; cmpxchgl %edx, (%ebx)");
    __asm volatile("jnz 1b");
    __asm volatile("popa");
}

void leave_critical_section()
{
    __asm volatile("pusha");
    __asm volatile("movl $0, (%0)" : : "r" (&mutex));
    __asm volatile("popa");
}

void* thread_func()
{
    for(int i = 0; i < 100000; ++i)
    {
        enter_critical_section();
        c++;       
        leave_critical_section();
    }
}

int main()
{
    pthread_t threads[4];
    
    for (int j = 0; j < 20; ++j)
    {
        c = 0;
        for (int i = 0; i < 4; ++i)
            pthread_create(&threads[i], NULL, thread_func, NULL);

        for (int i = 0; i < 4; ++i)
            pthread_join(threads[i], NULL);

        if (c != 400000)
        {
            printf("Threads are not syncronized\n");
            abort();
        }
    }

    printf("Treads are syncronized\n");
    return 0;
}
