using System;
using System.Diagnostics;
using System.Threading;

namespace Threadings
{
    internal class Program
    {
        private static long _sum;
        private static readonly object _sumLock = new object();
        private static int _length = 1_000_000;

        private static void Main()
        {
            //    var t1 = new Thread(Locked1);
            //    var t2 = new Thread(Locked2);

            var t1 = new Thread(Lockless1);
            var t2 = new Thread(Lockless2);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine(_sum);
        }

        private static void Locked1()
        {
            lock (_sumLock)
            {
                Lockless1();
            }
        }

        private static void Lockless1()
        {
            for (var i = 1; i <= _length / 2; i++)
            {
                _sum += i;
            }
        }

        private static void Locked2()
        {
            lock (_sumLock)
            {
                Lockless2();
            }
        }

        private static void Lockless2()
        {
            for (var i = _length / 2 + 1; i <= _length; i++)
            {
                _sum += i;
            }
        }
    }
}