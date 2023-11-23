using System;
using System.Reflection;

namespace Unit;


class MyUnit
{
    static void Main(string[] args)
    {
        if (args.Count<string>() == 1)
        {
            Console.WriteLine("Running tests");
        }
        else
        {
            Console.WriteLine("Incorrect arguments count");
        }

        Console.Read();
    }
}