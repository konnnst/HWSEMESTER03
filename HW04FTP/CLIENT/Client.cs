using System;
using System.IO;

namespace Client;
internal class MyClient
{
    static void Main()
    {
        var client = new ClientNetwork();
        client.SendCommands();
        Console.ReadKey();
    } 
}