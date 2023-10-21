using System.Net.Sockets;

public class ClientNetwork
{
    private int _port;

    public ClientNetwork()
    {   
        _port = 1488;
    }
    public ClientNetwork(int port)
    {
        _port = port;
    }

    public void SendMessage()
    {
        using (var client = new TcpClient("localhost", _port))
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);

            writer.Write("Hello server");
            writer.Flush();
        }
    }
}