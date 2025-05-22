
using System;
using System.Threading;
using System.Collections.Generic;

namespace DiningPhilosophers.Solution1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Table table = new Table();

            for (int i = 0; i < 5; i++)
            {
                new Philosopher(i, table);
            }

        }
    }

    public class Philosopher
    {
        private readonly Table _table;
        private readonly int _leftFork, _rightFork;
        private readonly int _id;
        private Thread _thread;

        public Philosopher(int id, Table table)
        {
            _id = id;
            _table = table;

            if (id < 4)
            {
                _rightFork = id;
                _leftFork = (id + 1) % 5;
            }
            else
            {
                _leftFork = id;
                _rightFork = (id + 1) % 5;
            }

            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Run()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Philosopher {_id} is thinking {i + 1} times");
                _table.GetFork(_rightFork);
                _table.GetFork(_leftFork);
                Console.WriteLine($"Philosopher {_id} is eating {i + 1} times");
                _table.PutFork(_leftFork);
                _table.PutFork(_rightFork);
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
}