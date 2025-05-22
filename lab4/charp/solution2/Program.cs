
using System;
using System.Threading;
using System.Collections.Generic;

namespace DiningPhilosophers.Solution2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Table table = new Table();
            Waiter waiter = new Waiter(4);

            for (int i = 0; i < 5; i++)
            {
                new Philosopher(i, table, waiter);
            }
        }
    }

    public class Philosopher
    {
        private readonly int _id;
        private readonly int _leftFork, _rightFork;
        private readonly Table _table;
        private readonly Waiter _waiter;
        private Thread _philosopher;

        public Philosopher(int id, Table table, Waiter waiter)
        {
            _id = id;
            _table = table;
            _waiter = waiter;
            _leftFork = id;
            _rightFork = (id + 1) % 5; 

            _philosopher = new Thread(Run);
            _philosopher.Start();
        }

        public void Run()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Philosopher {_id} is thinking {i + 1} times");
                _waiter.RequestPermission();

                _table.GetFork(_leftFork);
                _table.GetFork(_rightFork);
                Console.WriteLine($"Philosopher {_id} is eating {i + 1} times");
                _table.PutFork(_rightFork); 
                _table.PutFork(_leftFork);

                _waiter.ReleasePermission();
            }
        }
    }

    public class Table
    {
        private readonly Semaphore[] _forks = new Semaphore[5];

        public Table()
        {
            for (int i = 0; i < _forks.Length; i++)
            {
                _forks[i] = new Semaphore(1, 1); 
            }
        }

        public void GetFork(int id)
        {
            _forks[id].WaitOne();
        }

        public void PutFork(int id)
        {
            _forks[id].Release();
        }
    }

    public class Waiter
    {
        private readonly Semaphore _tokens;

        public Waiter(int maxConcurrentPhilosophers)
        {
            _tokens = new Semaphore(maxConcurrentPhilosophers, maxConcurrentPhilosophers);
        }

        public void RequestPermission()
        {
            _tokens.WaitOne();
        }

        public void ReleasePermission()
        {
            _tokens.Release();
        }
    }
}