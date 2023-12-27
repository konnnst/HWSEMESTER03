using System;
using System.IO;

namespace Client;
internal class MyClient
{
    static async Task Main()
    {
        await ClientUI.Start();
    } 
}