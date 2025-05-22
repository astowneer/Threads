using System;
using System.Threading;

namespace DiningPhilosophers.Solution4
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

                while (!_table.PickBothForks(_leftFork, _rightFork))
                {
                    Thread.Sleep(50);
                }

                Console.WriteLine($"Philosopher {_id} is eating {i + 1} times");
                _table.PutDownForks(_leftFork, _rightFork);
            }
        }
    }

    public class Table
    {
        private readonly SemaphoreSlim[] _forks = new SemaphoreSlim[5];
        private readonly object _tableLock = new object();

        public Table()
        {
            for (int i = 0; i < _forks.Length; i++)
            {
                _forks[i] = new SemaphoreSlim(1, 1); 
            }
        }

        public bool PickBothForks(int left, int right)
        {
            lock (_tableLock)
            {
                if (_forks[left].CurrentCount == 1 && _forks[right].CurrentCount == 1)
                {
                    _forks[left].Wait();
                    _forks[right].Wait();
                    return true;
                }
                return false; 
            }
        }

        public void PutDownForks(int left, int right)
        {
            _forks[left].Release();
            _forks[right].Release();
        }
    }
}