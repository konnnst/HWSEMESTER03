global using System;
global using System.Reflection;

namespace Unit;

class MyUnit
{
    static void Main(string[] args)
    {
        if (args.Count<string>() == 1)
        {
            TestLauncher.RunTests(args[0]);
        }
        else
        {
            Console.WriteLine("Incorrect arguments count");
        }
    }
}