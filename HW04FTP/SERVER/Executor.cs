namespace Server;

/// <summary>
/// Executing already parsed commands from command set
/// </summary>
public static class Executor
{
    /// <summary>
    /// Executing command in parsed form
    /// </summary>
    /// <param name="commandType"></param>
    /// <param name="path"></param>
    /// <returns>Raw string response</returns>
    public static string RespondCommand(string commandType, string path)
    {
        string response;
        if (commandType == "get")
        {
            response = Commands.Get(path);
        }
        else if (commandType == "list")
        {
            response = Commands.List(path);
        }
        else
        {
            response = "ACHTUNG! ACHTUNG! INCORRECT COMMAND!";
        }

        return response;
    }

}
