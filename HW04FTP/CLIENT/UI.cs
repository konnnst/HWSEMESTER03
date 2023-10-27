namespace Client;

/// <summary>
/// Simple user interface for client
/// </summary>
public static class ClientUI
{
    /// <summary>
    /// Starts client command loops
    /// </summary>
    public static void Start()
    {
        var run = true;
        var connection = new ClientNetwork();

        while (run)
        {
            Console.Write("ftp_client > ");
            var query = Console.ReadLine();
            var response = connection.GetServerResponse(query);
            Console.WriteLine(response);
        }
    }
}