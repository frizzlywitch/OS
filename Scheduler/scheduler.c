#include <ucontext.h>
#include <signal.h>
#include <unistd.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

ucontext_t* context[2];
int cont_num = 0;
ucontext_t* scheduler_context;

void sheduler(ucontext_t* rem_context)
{   
    printf("Scheduler start\n");

    context[cont_num] = rem_context;
    cont_num ^= 1;
    ucontext_t* new_context = context[cont_num];

    if(new_context == NULL)
    {
        if(rem_context == NULL)
            exit(0);

        new_context = rem_context;
        cont_num ^= 1;
    }

    alarm(1);
    printf("Try set context %d\n", cont_num);
    setcontext(new_context);
}

void factorial(int number)
{
    printf("Start Factorial of %d\n", number);
    int i;
    for (i = 0; i < 10000000; ++i)
    {
        int n = number;
        size_t fact = 1;
        while(n > 0)
        {
            fact *= n;
            --n;
        }
    }
    printf("Stop Factorial of %d\n", number);
}

void make_processes()
{
    context[0] = (ucontext_t*) calloc(1, sizeof(ucontext_t));
    context[1] = (ucontext_t*) calloc(1, sizeof(ucontext_t));
    void* stack1 = calloc(4096, sizeof(char));
    void* stack2 = calloc(4096, sizeof(char));
    
    if(context[0] == NULL || context[1] == NULL || stack1 == NULL || stack2 == NULL)
    {
        printf("Memory allocation failed\n");
        abort();
    }

    getcontext(context[0]);
    context[0]->uc_stack.ss_sp = stack1;
    context[0]->uc_stack.ss_size = 4096;
    context[0]->uc_link = scheduler_context;
    makecontext(context[0], (void(*)(void))factorial, 1, 100);

    getcontext(context[1]);
    context[1]->uc_stack.ss_sp = stack2;
    context[1]->uc_stack.ss_size = 4096;
    context[1]->uc_link = scheduler_context;
    makecontext(context[1], (void(*)(void))factorial, 1, 100);
}

void make_sheduler()
{
    scheduler_context = (ucontext_t*) calloc(1, sizeof(ucontext_t));
    getcontext(scheduler_context);
    void* stack = calloc(4096, sizeof(char));
    scheduler_context->uc_stack.ss_sp = stack;
    scheduler_context->uc_stack.ss_size = 4096;
    makecontext(scheduler_context, (void(*)(void))sheduler, 0);
}

void alarm_handler(int n, siginfo_t* sinfo, void* context)
{
    sheduler((ucontext_t*) context);
}

void set_handler_for_alarm_action()
{
    struct sigaction alarm_action;
    memset(&alarm_action, 0, sizeof(struct sigaction));
    alarm_action.sa_sigaction = alarm_handler;
    alarm_action.sa_flags = SA_SIGINFO;
    sigaction(SIGALRM, &alarm_action, NULL);
}

int main()
{
    make_sheduler();
    make_processes();
    set_handler_for_alarm_action();
    sheduler(context[0]);
    return 0;
}
