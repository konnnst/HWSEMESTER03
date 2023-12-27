using System.Net.Sockets;

namespace Client;

/// <summary>
/// Managing connection with server
/// </summary>
public class ClientNetwork
{
    private int _port;
    private string _address;
    private TcpClient _client;

    private TcpClient GetTcpClient()
    {
        TcpClient client;
        try
        {
            client = new TcpClient();
            return client;
        }   
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot connect to {_port} on {_address} address");
            Console.WriteLine($"Exception message: {ex.Message}");
        }
        throw new Exception("Client stayed unassigned for some reason");
    }
    public ClientNetwork()
    {
        _port = 1337;
        _address = "localhost";
        _client = GetTcpClient();
        _client.ConnectAsync(_address, _port);
    }

    public ClientNetwork(string address)
    {
        _port = 1337;
        _address = address;
        _client = GetTcpClient();

    }

    public ClientNetwork(int port)
    {
        _port = port;
        _address = "localhost";
        _client = GetTcpClient();
    }

    public ClientNetwork(int port, string address)
    {
        _port = port;
        _address = address;
        _client = GetTcpClient();

    }   

    /// <summary>
    /// Communication with server
    /// </summary>
    /// <param name="query">Raw command string</param>
    /// <returns>Response string</returns>
    public async Task<string> GetServerResponse(string query)
    {
        try
        {
            var stream = _client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            await writer.WriteLineAsync(query);
            await writer.FlushAsync();

            var response = reader.ReadLine();

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Something went wrong");
            Console.WriteLine($"Exception message: {ex.Message}");
        }
        throw new Exception("Response stayed unassigned for some reason");
    }
}