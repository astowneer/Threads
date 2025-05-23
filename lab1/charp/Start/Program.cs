﻿// using System;
// using System.Threading;

// namespace ThreadDemo
// {
//     class BreakControllerThread
//     {
//         int[] delays;
//         BreakFlag[] flags;

//         public BreakControllerThread(int[] delays, BreakFlag[] flags)
//         {
//             this.delays = delays;
//             this.flags = flags;
//         }

//         public void Start()
//         {
//             for (int i = 0; i < delays.Length; i++)
//             {
//                 Thread.Sleep(delays[i]);
//                 flags[i].Stop();
//             }
//         }
//     }

//     class BreakFlag
//     {
//         private bool stop = false;

//         public bool ShouldStop()
//         {
//             return stop;
//         }

//         public void Stop()
//         {
//             this.stop = true;
//         }
//     }

//     class WorkerThread
//     {
//         int threadId;
//         int step;
//         BreakFlag flag;

//         public WorkerThread(int threadId, int step, BreakFlag flag)
//         {
//             this.threadId = threadId;
//             this.flag = flag;
//             this.step = step;
//         }

//         public void Calculate()
//         {
//             long sum = 0;
//             int count = 0;
//             while (!flag.ShouldStop())
//             {
//                 sum += step;
//                 count++;
//             }

//             Console.WriteLine($"Thread {threadId} Sum: {sum} Count: {count}");
//         }
//     }

//     class Program
//     {
//         static void Main(string[] args)
//         {
//             int numberOfThreads = 3;
//             int step = 1;
//             int[] delays = new int[] { 2000, 1000, 4000 };

//             Thread[] threads = new Thread[numberOfThreads];
//             BreakFlag[] flags = new BreakFlag[numberOfThreads];
//             for (int i = 0; i < delays.Length; i++)
//             {
//                 flags[i] = new BreakFlag();
//                 WorkerThread worker = new WorkerThread(i + 1, step, flags[i]);
//                 threads[i] = new Thread(worker.Calculate);
//                 threads[i].Start();
//             }

//             int[] indices = SortOrderIndices(delays);
//             ReorderDelaysAndFlags(indices, ref delays, ref flags);
//             NormalizeDelaysToDifferences(ref delays);

//             BreakControllerThread controllerThread = new BreakControllerThread(delays, flags);
//             controllerThread.Start();

//             for (int i = 0; i < delays.Length; i++)
//             {
//                 threads[i].Join();
//             }

//             Console.WriteLine("All threads have finished.");
//         }

//         private static void NormalizeDelaysToDifferences(ref int[] delays)
//         {
//             for (int i = 1; i < delays.Length; i++)
//             {
//                 delays[i] -= delays[i - 1];
//             }
//         }

//         private static void ReorderDelaysAndFlags(int[] sortedIndices, ref int[] delays, ref BreakFlag[] flags)
//         {
//             int[] sortedDelays = (int[])delays.Clone();
//             BreakFlag[] sortedFlags = (BreakFlag[])flags.Clone();

//             for (int i = 0; i < sortedIndices.Length; i++)
//             {
//                 delays[i] = sortedDelays[sortedIndices[i]];
//                 flags[i] = sortedFlags[sortedIndices[i]];
//             }
//         }

//         private static int[] SortOrderIndices(int[] array)
//         {
//             int[] indices = GenerateArraySequence(array.Length);
//             Array.Sort(indices, (a, b) => array[a].CompareTo(array[b]));
//             return indices;
//         }

//         public static int[] GenerateArraySequence(int length)
//         {
//             int[] sequence = new int[length];
//             for (int i = 0; i < length; i++)
//             {
//                 sequence[i] = i;
//             }
//             return sequence;
//         }
//     }
// }


using System;
using System.Threading;

namespace ThreadSumSharp
{
    class Program
    {
        private static readonly int dim = 10000000;
        private static readonly int threadNum = 3;

        private readonly Thread[] thread = new Thread[threadNum];

        static void Main(string[] args)
        {
            Program main = new Program();
            main.InitArr();
            long[] min1 = main.PartMin(0, dim);
            Console.WriteLine("Min " + min1[0] + " index " + min1[1]);

            long[] min2 = main.ParallelMin();
            Console.WriteLine("Min " + min2[0] + " index " + min2[1]);
        }

        private int threadCount = 0;

        private long[] ParallelMin()
        {
            int[][] indexes = GetSplitIndexes();
            for (int i = 0; i < indexes.Length; i++)
            {
                Console.WriteLine("Part " + (i + 1) + ": start = " + indexes[i][0] + ", end = " + indexes[i][1]);
            }

            ChangeElementByIndex(6666667, -38);
            ChangeElementByIndex(3333334, -40);
            ChangeElementByIndex(9999999, -45);

            for (int i = 0; i < threadNum; i++)
            {
                thread[i] = new Thread(StarterThread);
                thread[i].Start(new Bound(indexes[i][0], indexes[i][1] + 1));
            }

            lock (lockerForCount)
            {
                while (threadCount < threadNum)
                {
                    Monitor.Wait(lockerForCount);
                }
            }
            return new long[] { min, indexMin };
        }

        private readonly int[] arr = new int[dim];

        private void InitArr()
        {
            for (int i = 0; i < dim; i++)
            {
                arr[i] = i;
            }
        }

        class Bound
        {
            public Bound(int startIndex, int finishIndex)
            {
                StartIndex = startIndex;
                FinishIndex = finishIndex;
            }

            public int StartIndex { get; set; }
            public int FinishIndex { get; set; }
        }

        private readonly object lockerForMin = new object();
        private void StarterThread(object param)
        {
            if (param is Bound)
            {
                long[] min = PartMin((param as Bound).StartIndex, (param as Bound).FinishIndex);

                lock (lockerForMin)
                {
                    PutMin(min);
                }
                IncThreadCount();
            }
        }

        private readonly object lockerForCount = new object();
        private void IncThreadCount()
        {
            lock (lockerForCount)
            {
                threadCount++;
                Monitor.Pulse(lockerForCount);
            }
        }

        private long min = int.MaxValue;
        private long indexMin = 0;
        public void PutMin(long[] min)
        {
            if (this.min > min[0])
            {
                this.min = min[0];
                this.indexMin = min[1];
            }
        }

        public long[] PartMin(int startIndex, int finishIndex)
        {
            long min = arr[startIndex];
            long indexMin = startIndex;
            for (int i = startIndex + 1; i < finishIndex; i++)
            {
                if (min > arr[i])
                {
                    min = arr[i];
                    indexMin = i;
                }
            }
            return [min, indexMin];
        }

        public void ChangeElementByIndex(int index, int value)
        {
            if (index < 0 || index >= arr.Length)
            {
                throw new IndexOutOfRangeException($"Index {index} is out of bounds for array of length {arr.Length}");
            }
            arr[index] = value;
        }

        public int[][] GetSplitIndexes()
        {
            int[][] result = new int[threadNum][];
            int baseSize = dim / threadNum;
            int remainder = dim % threadNum;

            int startIndex = 0;

            for (int i = 0; i < threadNum; i++)
            {
                int partSize = baseSize + (i < remainder ? 1 : 0);
                int endIndex = startIndex + partSize - 1;
                result[i] = new int[] { startIndex, endIndex };
                startIndex = endIndex + 1;
            }

            return result;
        }
    }
}