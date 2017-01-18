using static System.Console;

namespace Appson.Common.Threading.Tests.Throttler
{
    internal class ThrottlerMenu
    {
        public void Run()
        {
            WriteLine("Please select throttler test scenario:");
            WriteLine("    1. Simple test");
            WriteLine("    2. Multi-threaded test");
            WriteLine("    3. Simple async test");
            WriteLine("    4. TPL Dataflow async test");
            WriteLine();

            var choice = ReadLine();
            switch (choice)
            {
                case "1":
                    new ThrottlerSimpleTest().Run();
                    break;
                case "2":
                    new ThrottlerMultiThreadedTest().Run();
                    break;
                case "3":
                    new ThrottlerSimpleAsyncTest().Run();
                    break;
                case "4":
                    new ThrottlerTplDataflowAsyncTest().Run();
                    break;
            }
        }
    }
}
