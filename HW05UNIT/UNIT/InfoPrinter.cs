using Unit;

public static class InfoPrinter
{
    public static void PrintClassInfo(TestClassInfo testClassInfo)
    {
        Console.WriteLine($"Class {testClassInfo.ClassName}");
        Console.WriteLine($"Detected: tests - {testClassInfo.TestMethodsCount}, " +
                            $"before - {testClassInfo.BeforeCount}, " + 
                            $"after - {testClassInfo.AfterCount}, " + 
                            $"beforeClass - {testClassInfo.BeforeClassCount}, " + 
                            $"afterClass - {testClassInfo.AfterClassCount}");
        Console.ForegroundColor = ConsoleColor.Red;
        foreach (var failedMethodInfo in testClassInfo.Failed)
        {
            Console.WriteLine($"Failed {failedMethodInfo.Name} (message: {failedMethodInfo.Message}) in {failedMethodInfo.TimeElapsed} ms");
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (var skippedMethodInfo in testClassInfo.Skipped)
        {
            Console.WriteLine($"Skipped {skippedMethodInfo.Name} (message: {skippedMethodInfo.Message})");
        }
        if (testClassInfo.Failed.Count != 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        Console.WriteLine($"Passed - {testClassInfo.Correct.Count}, " +
                        $"failed - {testClassInfo.Failed.Count}, " +
                        $"skipped - {testClassInfo.Skipped.Count}\n" +
                        $"Elapsed time - {testClassInfo.totalTimeElapsed} ms");
        Console.ForegroundColor = ConsoleColor.White;

    }
    public static void PrintBuildInfo(BuildInfo buildInfo)
    {
        Console.WriteLine($"Test results for {buildInfo.BuildName}");
        foreach (var classInfo in buildInfo.TestClasses)
        {
            PrintClassInfo(classInfo);
            Console.WriteLine();
        }
    }

    public static void PrintInfo(List<BuildInfo> buildInfoList)
    {
        foreach (var buildInfo in buildInfoList)
        {
            PrintBuildInfo(buildInfo);
            Console.WriteLine();
        }
    }
}