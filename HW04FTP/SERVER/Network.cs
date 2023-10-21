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
    public void ProcessMessages()
    {
        var listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();

        using (var socket = listener.AcceptSocket())
        {
            var stream = new NetworkStream(socket);
            var reader = new StreamReader(stream);

            var message = reader.ReadToEnd();
            Console.WriteLine(message);
        }

        listener.Stop();
    }
}