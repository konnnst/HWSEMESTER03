using System;

namespace Client;

// Куда лучше это запихнуть: на сервер или на клиент? или вообще в задницу???)
/// <summary>
/// Checks if users query correct and parses command and path
/// to corresponding fields
/// </summary>
public class Parser
{
    private string? _commandType;
    private string? _path;

    public string? CommandType => _commandType;
    public string? Path => _path;

    public void Parse(string rawCommand)
    {
        string[] commandParts = rawCommand.Split();

        if (commandParts[0] == "get" || commandParts[0] == "list")
        {
            _commandType = commandParts[0];
        }
        else
        {
            throw new Exception("Incorrect command word");
        }

        if (commandParts.Count<string>() != 2)
        {
            throw new IOException("Incorrect parameters count");
        }

        if (CheckIfPathValid(commandParts[1]))
        {
            _path = commandParts[1];
        }
        else
        {
            throw new Exception("Incorrect path format");
        }
    }

    private bool CheckIfPathValid(string path)
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