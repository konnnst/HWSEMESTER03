namespace Server;

public static class Executor
{
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
