using Appson.Common.Threading.Tests.General;
using Appson.Common.Threading.Tests.Throttler;
using static System.Console;

namespace Threading.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        public void Run()
        {
            WriteLine("Please select menu:");
            WriteLine("    1. General utilities");
            WriteLine("    2. Throttler tests");
            WriteLine();

            var choice = ReadLine();
            switch (choice)
            {
                case "1":
                    new GeneralMenu().Run();
                    break;

                case "2":
                    new ThrottlerMenu().Run();
                    break;
            }
        }
    }
}
