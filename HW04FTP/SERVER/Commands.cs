namespace Server;

public static class Commands
{
    public static string Get(string path)
    {
        var response = "";

        if (!File.Exists(path))
        {
            response = "-1";
        }
        else
        {
            var file = new FileInfo(path);

            response += $"{file.Length} ";
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
            response = "-1";
        }
        else
        {
            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);

            response += $"{files.Count() + directories.Count()} ";

            foreach (var fileName in files)
            {
                response += $"f {fileName} ";
            }

            foreach (var directoryName in directories)
            {
                response += $"d {directoryName} ";
            }
        }

        return response;
    }
}
