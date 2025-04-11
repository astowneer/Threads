using System;
using System.Threading;

namespace ThreadDemo
{
    class BreakControllerThread
    {
        int[] delays;
        BreakFlag[] flags;

        public BreakControllerThread(int[] delays, BreakFlag[] flags)
        {
            this.delays = delays;
            this.flags = flags;
        }

        public void Start()
        {
            for (int i = 0; i < delays.Length; i++)
            {
                Thread.Sleep(delays[i]);
                flags[i].Stop();
            }
        }
    }

    class BreakFlag
    {
        private bool stop = false;

        public bool ShouldStop()
        {
            return stop;
        }

        public void Stop()
        {
            this.stop = true;
        }
    }

    class WorkerThread
    {
        int threadId;
        int step;
        BreakFlag flag;

        public WorkerThread(int threadId, int step, BreakFlag flag)
        {
            this.threadId = threadId;
            this.flag = flag;
            this.step = step;
        }

        public void Calculate()
        {
            long sum = 0;
            int count = 0;
            while (!flag.ShouldStop())
            {
                sum += step;
                count++;
            }

            Console.WriteLine($"Thread {threadId} Sum: {sum} Count: {count}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int numberOfThreads = 3;
            int step = 1;
            int[] delays = new int[] { 2000, 1000, 4000 };

            Thread[] threads = new Thread[numberOfThreads];
            BreakFlag[] flags = new BreakFlag[numberOfThreads];
            for (int i = 0; i < delays.Length; i++)
            {
                flags[i] = new BreakFlag();
                WorkerThread worker = new WorkerThread(i + 1, step, flags[i]);
                threads[i] = new Thread(worker.Calculate);
                threads[i].Start();
            }
          
            int[] indices = SortOrderIndices(delays);
            ReorderDelaysAndFlags(indices, ref delays, ref flags);
            NormalizeDelaysToDifferences(ref delays);

            BreakControllerThread controllerThread = new BreakControllerThread(delays, flags);
            controllerThread.Start();
     
            for (int i = 0; i < delays.Length; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine("All threads have finished.");
        }

        private static void NormalizeDelaysToDifferences(ref int[] delays)
        {
            for (int i = 1; i < delays.Length; i++)
            {
                delays[i] -= delays[i - 1];
            }
        }

        private static void ReorderDelaysAndFlags(int[] sortedIndices, ref int[] delays, ref BreakFlag[] flags)
        {
            int[] sortedDelays = (int[])delays.Clone();
            BreakFlag[] sortedFlags = (BreakFlag[])flags.Clone();

            for (int i = 0; i < sortedIndices.Length; i++)
            {
                delays[i] = sortedDelays[sortedIndices[i]];
                flags[i] = sortedFlags[sortedIndices[i]];
            }
        }

        private static int[] SortOrderIndices(int[] array)
        {
            int[] indices = GenerateArraySequence(array.Length);
            Array.Sort(indices, (a, b) => array[a].CompareTo(array[b]));
            return indices;
        }

        public static int[] GenerateArraySequence(int length)
        {
            int[] sequence = new int[length];
            for (int i = 0; i < length; i++)
            {
                sequence[i] = i;
            }
            return sequence;
        }
    }
}

// WorkerThread worker1 = new WorkerThread(1, step, flags[0]);
// WorkerThread worker2 = new WorkerThread(2, step, flags[1]);
// Thread thread1 = new Thread(worker1.Calculate);
// Thread thread2 = new Thread(worker2.Calculate);

// thread1.Start();
// thread2.Start();