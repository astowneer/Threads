
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