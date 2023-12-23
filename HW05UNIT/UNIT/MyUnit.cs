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
            TestLauncher.RunTests("/home/konnnst/Desktop/hw/HW05UNIT/resources/testproj/proj/bin/Debug/net8.0/");
            //Console.WriteLine("Incorrect arguments count"); 
        }
    }
}