using System;
using System.Security.Cryptography;

namespace Appson.Common.Threading.Tests.General
{
    internal class BusyWaitTest
    {
        public void Run()
        {
            var random = new Random();
            var sha = new SHA256Managed();

            while (true)
            {
                var next = new byte[100];
                random.NextBytes(next);
                for (int i = 0; i < 200000; i++)
                {
                    next = sha.ComputeHash(next);
                }

                Console.WriteLine(Convert.ToBase64String(next));
            }
        }
    }
}