using System;
using System.Threading;

namespace threaddemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfThreads = 5;
            int step = 1;

            BreakFlag[] flags = new BreakFlag[numberOfThreads];
            Thread[] threads = new Thread[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++)
            {
                flags[i] = new BreakFlag();
                int threadId = i;
                threads[i] = new Thread(() => WorkerThread(threadId, step, flags[threadId]));

                Thread controllerThread = new Thread(() => ControllerThread(flags[threadId], 5000));
                threads[i].Start();
                controllerThread.Start();
            }
        }

        static void WorkerThread(int id, int step, BreakFlag flag)
        {
            long sum = 0;
            long count = 0;
            while (!flag.ShouldStop())
            {
                sum += step;
                count++;
            }
            Console.WriteLine($"Thread{id} Sum: {sum} Count: {count}");
        }

        static void ControllerThread(BreakFlag flag, int delayMs)
        {
            Thread.Sleep(delayMs);
            flag.Stop();
        }
    }
}
