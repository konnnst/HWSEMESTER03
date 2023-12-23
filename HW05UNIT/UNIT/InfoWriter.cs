namespace Unit;


/// <summary>
/// Writes stats and messages in pretty form using templates
/// </summary>
public static class InfoWriter
{
    public static void WriteStatsSpecialMethods(TestClassInfo info, Action? beforeClass, Action? afterClass,
                                    Action? before, Action? after, List<Action> testMethods)
    {
        info.TestMethodsCount = testMethods.Count();
        info.BeforeClassCount = Convert.ToInt32(beforeClass != null);
        info.AfterClassCount = Convert.ToInt32(afterClass != null);
        info.BeforeCount = Convert.ToInt32(before != null);
        info.AfterCount = Convert.ToInt32(after != null);
    }
}