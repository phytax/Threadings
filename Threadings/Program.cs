using System;
using System.Diagnostics;
using System.Threading;

namespace Threadings
{
    internal class Program
    {
        private static long _sum;
        private static readonly object SumLock = new object();
        private const int Length = 1_000_000;

        private static void Main()
        {
            var sw = new Stopwatch();
            sw.Start();

//            var t1 = new Thread(Locked1);
//            var t2 = new Thread(Locked2);

             var t1 = new Thread(Lockless1);
             var t2 = new Thread(Lockless2);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine($"{sw.ElapsedTicks,10} | Result: {_sum, 30}");
        }

        private static void Locked1()
        {
            lock (SumLock)
            {
                Lockless1();
            }
        }

        private static void Lockless1()
        {
            for (var i = 1; i <= Length / 2; i++)
            {
                _sum += i;
            }
        }

        private static void Locked2()
        {
            lock (SumLock)
            {
                Lockless2();
            }
        }

        private static void Lockless2()
        {
            for (var i = Length / 2 + 1; i <= Length; i++)
            {
                _sum += i;
            }
        }
    }
}