using System.Net.Sockets;
using System.Net;

namespace Server;
class ServerNetwork
{
    private int _port;

    public ServerNetwork()
    {   
        _port = 1488;
    }
    public ServerNetwork(int port)
    {
        _port = port;
    }
    public void RespondCommands()
    {
        var listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();

        using (var socket = listener.AcceptSocket())
        {
            var stream = new NetworkStream(socket);
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            string response;

            while (true)
            {
                var query = reader.ReadLine();
                
                if (query == null)
                {
                    response = "Empty query";
                }
                else if (Parser.Parse(query))
                {
                    response = Executor.RespondCommand(Parser.CommandType, Parser.Path);
                }
                else
                {
                    response = "Incorrect query format";
                }

                writer.WriteLine(response);
                writer.Flush();
            }
        }

        listener.Stop();
    }
}