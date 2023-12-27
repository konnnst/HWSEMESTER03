namespace Server;

/// <summary>
/// Contains commands from server commands set
/// </summary>
public static class Commands
{
    /// <summary>
    /// -1 if file not found, else
    /// size:Long contents: Bytes
    /// </summary>
    /// <param name="path">Absolute or relative path of target file</param>
    /// <returns>Response string</returns>
    public static async Task<string> Get(string path)
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
            foreach (var bytePiece in await File.ReadAllBytesAsync(path))
            {
                response += $"{bytePiece} ";
            }

            response = response.Trim();
        }

        return response;
    }

    /// <summary>
    /// Response format:
    /// -1 if directory not exists else
    /// size f name1 ... d nameN
    /// f -- if file, d -- if directory
    /// </summary>
    /// <param name="path"></param>
    /// <returns>Response string</returns>
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

            response = response.Trim();
        }

        return response;
    }
}
