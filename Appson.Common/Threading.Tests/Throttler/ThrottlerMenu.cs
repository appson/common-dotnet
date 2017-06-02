using static System.Console;

namespace Appson.Common.Threading.Tests.Throttler
{
    internal class ThrottlerMenu
    {
        public void Run()
        {
            WriteLine("Please select throttler test scenario:");
            WriteLine("    1. simple test");
            WriteLine("    2. throttled Enumerable test");
            WriteLine();

            var choice = ReadLine();
            switch (choice)
            {
                case "1":
                    new ThrottlerSimpleTest().Run();
                    break;
                case "2":
                    new EnumerableThrottlingTest().Run();
                    break;
            }
        }
    }
}