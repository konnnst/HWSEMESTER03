namespace Unit;


/// <summary>
/// Writes stats and messages in pretty form using templates
/// </summary>
public static class InfoWriter
{
    public static void WriteStatsSpecialMethods(Type t, Action? beforeClass, Action? afterClass,
                                    Action? before, Action? after, List<Action> testMethods)
    {
        Console.WriteLine("Class {0}: " +
                        "test methods - {1}, " +
                        "beforeClass - {2}, " +
                        "afterClass - {3}, " +
                        "before - {4}, " +
                        "after - {5}", t.Name, testMethods.Count, Convert.ToInt32(beforeClass != null),
                        Convert.ToInt32(afterClass != null), Convert.ToInt32(before != null),
                        Convert.ToInt32(after != null));
    }

    public static void WriteSkipMessage(Action testMethod, string skipMessage)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Skipped {testMethod.Method.Name} test method. Message:\n\t{skipMessage}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteExceptionMessage(Action testMethod, string exceptionMessage, long elapsedTime)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed {testMethod.Method} [{elapsedTime} ms]: \n\t{exceptionMessage}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteTestResults(List<long> elapsedTime, List<Action> testMethods,
                int failedCount, int skippedCount)
    {
        if (failedCount != 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        Console.WriteLine($"Tests finished: " +
                        $"passed - {testMethods.Count() - failedCount - skippedCount}, " +
                        $"failed - {failedCount}, " +
                        $"skipped - {skippedCount}, " +
                        $"total time elapsed - {elapsedTime.Sum()} ms\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

}