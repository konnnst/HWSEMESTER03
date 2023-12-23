using System.Reflection;

namespace Unit;

/// <summary>
/// Runs tests for every assembly in chosen path
/// </summary>
public static class TestLauncher
{
    private static List<Assembly> assemblies = new();
    private static IEnumerable<string> GetAssemblyPaths(string path)
    {
        var dllFiles = Directory.EnumerateFiles(path,
        "*.dll", SearchOption.AllDirectories);
        return dllFiles;
    }

    private static bool IsAssemblyLoaded(string path)
    {
        foreach (var asm in assemblies)
        {
            if (asm.Location.Split("/").Last() == path.Split("/").Last())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Runs tests for every assembly in chosen path
    /// </summary>
    /// <param name="path">Path to folder, holding the assemblies</param>
    public static void RunTests(string path)
    {
        var paths = GetAssemblyPaths(path);
        var buildStats = new List<BuildInfo>();

        foreach (var asm_path in paths)
        {
            if (!IsAssemblyLoaded(asm_path))
            {
                assemblies.Add(Assembly.LoadFrom(asm_path));
            }
        }

        foreach (var asm in assemblies)
        {
            var tester = new Tester(asm);
            Console.WriteLine($"Running tests for {asm.Location}");
            var buildInfo = tester.RunTests();
            buildStats.Add(buildInfo);
        }

        InfoPrinter.PrintInfo(buildStats);
    }
}

