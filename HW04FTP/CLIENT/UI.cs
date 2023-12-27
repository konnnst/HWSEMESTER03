namespace Client;

/// <summary>
/// Simple user interface for client
/// </summary>
public static class ClientUI
{
    /// <summary>
    /// Starts client command loops
    /// </summary>
    public static async Task Start()
    {
        var cancellationToken = new CancellationToken();
        var connection = new ClientNetwork();

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Write("ftp_client > ");
            var query = "list /";
            var response = await connection.GetServerResponse(query);
            Console.WriteLine(response);
        }
    }
}