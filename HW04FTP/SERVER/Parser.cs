using System;

namespace Server;

/// <summary>
/// Parse command in execution ready form
/// </summary>
public static class Parser
{
    private static string? _commandType;
    private static string? _path;

    /// <summary>
    /// Returns main command word
    /// </summary>
    public static string? CommandType => _commandType;
    /// <summary>
    /// Return path used in command
    /// </summary>
    public static string? Path => _path;

    /// <summary>
    /// Parse raw command to Parser object properties as CommandType and Path
    /// </summary>w
    /// <param name="rawCommand"></param>
    /// <returns>Returns true, if given string is correct, else false</returns>
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

    /// <summary>
    /// Takes path string and checks for incorrect symbols
    /// </summary>
    /// <param name="path">Path string</param>
    /// <returns>Returns true if string correct</returns>
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