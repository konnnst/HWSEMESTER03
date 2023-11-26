using System.Reflection;

namespace Unit;

public class TestLauncher
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

    public static void RunTests(string path)
    {
        var paths = GetAssemblyPaths(path);

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
                if (asm.ManifestModule.Name == "proj.dll")
                {
                    tester.RunTests();
                }
                Console.WriteLine();
        }
    }
}

