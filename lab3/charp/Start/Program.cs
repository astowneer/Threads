using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
sing System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProducerConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            int storageSize = 3;
            int producerAmount = 2;
            int producerItemNumbers = 8;
            int consumerAmount = 3;
            int consumerItemNumbers = 100;
            program.Starter(storageSize, producerAmount, producerItemNumbers, consumerAmount, consumerItemNumbers);
        }

        private void Starter(int storageSize, int producerAmount, int producerItemNumbers, int consumerAmount, int consumerItemNumbers)
        {
            Access = new Semaphore(1, 1);
            Full = new Semaphore(storageSize, storageSize);
            Empty = new Semaphore(0, storageSize);

            Thread[] producers = new Thread[producerAmount];
            for (int i = 0; i < producerAmount; i++)
            {
                producers[i] = new Thread(Producer);
                producers[i].Start(producerItemNumbers / producerAmount);
            }

            for (int i = 0; i < consumerAmount; i++)
            {
                Thread consumer = new Thread(Consumer);
                consumer.Start(consumerItemNumbers / consumerAmount);
            }

            for (int i = 0; i < producerAmount; i++)
            {
                producers[i].Join();
            }

            for (int i = 0; i < consumerAmount; i++)
            {
                storage.Add("poison");
                Empty.Release();
                Console.WriteLine($"Added poison pill {i + 1}");
            }
        }

        private Semaphore Access;
        private Semaphore Full;
        private Semaphore Empty;

        private readonly List<string> storage = new List<string>();

        private void Producer(Object itemNumbers)
        {
            int maxItem = 0;
            if (itemNumbers is int)
            {
                maxItem = (int)itemNumbers;
            }
            for (int i = 0; i < maxItem; i++)
            {
                Full.WaitOne();
                Access.WaitOne();

                storage.Add("item " + i);
                Console.WriteLine("Added item " + i);

                Access.Release();
                Empty.Release();
            }
        }

        private void Consumer(Object itemNumbers)
        {
            int maxItem = 0;
            if (itemNumbers is int)
            {
                maxItem = (int)itemNumbers;
            }
            for (int i = 0; i < maxItem; i++)
            {
                Empty.WaitOne();
                Thread.Sleep(1000);
                Access.WaitOne();

                string item = storage.ElementAt(0);
                storage.RemoveAt(0);

                if (item == "poison")
                {
                    Console.WriteLine("Took " + item);
                    break;
                }

                Full.Release();
                Access.Release();

                Console.WriteLine("Took " + item);
            }
        }
    }
}
