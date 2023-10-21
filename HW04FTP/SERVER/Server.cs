using System;

namespace Server;

public class MyServer
{
    static void Main()
    {
        var server = new ServerNetwork();
        server.ProcessMessages();
        Console.ReadKey();
    }
}