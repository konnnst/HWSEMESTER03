using System.Reflection;

namespace MultiThread;

public class MyConstants
{
    public static string CurrentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/";
    public static string MatrixPath = CurrentFolder + "matrix_{0}.txt";
    public static string ResultPath = CurrentFolder + "README.md";
}