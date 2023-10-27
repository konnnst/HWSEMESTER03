using System.Net.Sockets;
using System.Net;

namespace Server;

/// <summary>
/// Managing port and connection with clients
/// </summary>
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

    /// <summary>
    /// Starts listenting to port and sending responses to commands 
    /// </summary>
    public async void RespondCommands()
    {
        var listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();

        while (true)
        {
            var socket = await listener.AcceptSocketAsync();

            Task.Run(async () => {
                var stream = new NetworkStream(socket);
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);

                var query = await reader.ReadLineAsync();
                string response;

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

                await writer.WriteAsync(response);
                await writer.FlushAsync();

                socket.Close();
            });
        }
    }
}