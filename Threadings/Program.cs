using System;
using System.Threading;
using EnsureThat;

namespace Threadings
{
    internal class Program
    {
        private static long _sum;
        private static readonly object _sumLock = new object();
        private static int _length = 100_000_000;

        private static void Main(string[] args)
        {
            var theArg = GeFirstArg(args);

            Thread t1 = null;
            Thread t2 = null;

            switch (theArg)
            {
                case "locked":
                    t1 = new Thread(Locked1);
                    t2 = new Thread(Locked2);
                    break;

                case "lockless":
                    t1 = new Thread(Lockless1);
                    t2 = new Thread(Lockless2);

                    break;
            }

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

        private static string GeFirstArg(string[] args)
        {
            ValidateArgs(args);
            return args[0];
        }

        private static void ValidateArgs(string[] args)
        {
            var paras = "'locked' or 'lockless'";



            Ensure.Collection.HasItems(
                    args,
                    nameof(args),
                    opt => opt.WithMessage($"Please provide parameter {paras}"));

            Ensure.Collection.SizeIs(
                args,
                1,
                nameof(args),
                opt => opt.WithMessage($"One and only one parameter allowed: {paras}"));

            Ensure.That(
                args[0] == "locked" || args[0] == "lockless",
                nameof(args),
                opt => opt.WithMessage($"parameter must be {paras}"))
                .IsTrue();
        }
    }
}
