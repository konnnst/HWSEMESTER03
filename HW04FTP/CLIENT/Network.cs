using System.Net.Sockets;

namespace Client;
public class ClientNetwork
{
    private int _port;
    private string _address;

    public ClientNetwork()
    {
        _port = 1488;
        _address = "localhost";
    }

    public ClientNetwork(string address)
    {
        _port = 1488;
        _address = address;
    }

    public ClientNetwork(int port)
    {
        _port = port;
        _address = "localhost";
    }

    public ClientNetwork(int port, string address)
    {
        _port = port;
        _address = address;
    }   

    public string GetServerResponse(string query)
    {
        using (var client = new TcpClient(_address, _port))
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            writer.WriteLine(query);
            writer.Flush();

            var response = reader.ReadLine();

            return response;
        }
    }
}