
using System;
using System.Threading;

namespace DiningPhilosophers.Solution3
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
        private readonly int _id;
        private readonly int _leftFork, _rightFork;
        private readonly Table _table;
        private Thread _philosopher;

        public Philosopher(int id, Table table)
        {
            _id = id;
            _table = table;
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

                _table.GetFork(_leftFork);
                if (!_table.TryGetFork(_rightFork))
                {
                    _table.PutFork(_leftFork);
                    continue; 
                }

                Console.WriteLine($"Philosopher {_id} is eating {i + 1} times");
                _table.PutFork(_rightFork);
                _table.PutFork(_leftFork);
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

        public bool TryGetFork(int id)
        {
            return _forks[id].WaitOne(0); 
        }

        public void PutFork(int id)
        {
            _forks[id].Release();
        }
    }
}