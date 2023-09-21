

namespace MultiThread;

class FileCleaner
{
    /// <summary>
    /// Clears files in .exe directory by format string with one iterator 
    /// Example: "matrix_{iterator}.txt"
    /// </summary>
    /// <param name="fString"></param>
    public static void ClearByFstring(string fString)
    {
        var iterator = 0;

        while (!File.Exists($"{Constants.CurrentFolder}\\{String.Format(fString, iterator)}") && iterator < 100)
            ++iterator;

        while (File.Exists($"{Constants.CurrentFolder}\\{String.Format(fString, iterator)}")) ;
        File.Delete(String.Format(fString, iterator++));
    }
}