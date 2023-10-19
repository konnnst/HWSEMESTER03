using System;
using System.IO;

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
        char[] restrictedSymbols = { '<', '>', ']', '[', '(', ')', '\'', '"', ' '};
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

public class Commands
{
    public static string Get(string path)
    {
        var response = "";

        if (!File.Exists(path))
        {
            response = "-1\n";
        }
        else
        {
            var file = new FileInfo(path);

            response += $"{file.Length}\n";
            foreach (var bytePiece in File.ReadAllBytes(path))
            {
                response += $"{bytePiece} ";
            }
        }

        return response;
    }

    public static string List(string path)
    {
        var response = "";
        if (!Directory.Exists(path))
        {
            response = "-1\n";
        }
        else
        {
            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);

            response += $"{files.Count() + directories.Count()}\n";

            foreach (var fileName in files)
            {
                response += $"f {fileName}\n";
            }

            foreach (var directoryName in directories)
            {
                response += $"d {directoryName}\n";
            }
        }

        return response;
    }
}

public class Executor
{
    public string RespondCommand(string commandType, string path)
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

public class ClientTerminal
{
    public void RunClient()
    {
        var parser = new Parser();
        var executor = new Executor();
        var run = 1;
        while (run == 1)
        {
            Console.Write("simple_ftp > ");
            var command = Console.ReadLine();
            parser.Parse(command);
            var response = executor.RespondCommand(parser.CommandType, parser.Path);
            Console.WriteLine(response);
        }
    }
}

internal class MyClient
{
    static void Main()
    {
        Console.WriteLine(Commands.Get("Client.cs"));
        Console.ReadLine();
    } 

}