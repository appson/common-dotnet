using static System.Console;

namespace Appson.Common.Threading.Tests.General
{
    internal class GeneralMenu
    {
        public void Run()
        {
            WriteLine("Please select general scenario:");
            WriteLine("    1. Busy wait");
            WriteLine();

            var choice = ReadLine();
            switch (choice)
            {
                case "1":
                    new BusyWaitTest().Run();
                    break;
            }
        }
    }
}