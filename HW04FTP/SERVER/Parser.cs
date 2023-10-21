using System;

namespace Server;

public static class Parser
{
    private static string? _commandType;
    private static string? _path;

    public static string? CommandType => _commandType;
    public static string? Path => _path;

    public static bool Parse(string rawCommand)
    {
        string[] commandParts = rawCommand.Split();

        if (commandParts.Count<string>() != 2)
        {
            return false;
        }

        if (commandParts[0] == "get" || commandParts[0] == "list")
        {
            _commandType = commandParts[0];
        }
        else
        {
            return false;
        }

        if (CheckIfPathValid(commandParts[1]))
        {
            _path = commandParts[1];
        }
        else
        {
            return false;
        }

        return true;
    }

    private static bool CheckIfPathValid(string path)
    {
        char[] restrictedSymbols = { '<', '>', ']', '[', '(', ')', '\'', '"', ' ' };
        foreach (var symbol in path)
        {
            if (restrictedSymbols.Contains<char>(symbol))
            {
                return false;
            }
        }
        return true;
    }
}