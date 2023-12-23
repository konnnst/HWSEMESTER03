namespace Unit;

/// <summary>
/// Logs test results for passed test method
/// </summary>
public class PassedTestMethodInfo : TestMethodInfo
{
    public PassedTestMethodInfo(string name, long timeElapsed)
    {
        Name = name;
        TimeElapsed = timeElapsed;
    }
}

/// <summary>
/// Logs test results for failed test method
/// </summary>
public class FailedTestMethodInfo : TestMethodInfo
{
    public string Message { get; private set; } = "";

    public FailedTestMethodInfo(string name, long timeElapsed, string message)
    {
        Name = name;
        TimeElapsed = timeElapsed;
        Message = message;
    }
}

/// <summary>
/// Logs test results for skipped test method
/// </summary>
public class SkippedTestMethodInfo : TestMethodInfo
{
    public string Message { get; private set; } = "";

    public SkippedTestMethodInfo(string name, long timeElapsed, string message)
    {
        Name = name;
        TimeElapsed = timeElapsed;
        Message = message;
    }
}


public abstract class TestMethodInfo
{
    public string Name { get; protected set; } = "";
    public long TimeElapsed { get; protected set; }
}


/// <summary>
/// Logs test results for class marked as UnitTestClass
/// </summary>
public class TestClassInfo
{
    public string ClassName { get; private set; } = "";
    public int TestMethodsCount { get; set; }
    public int BeforeClassCount { get; set; }
    public int AfterClassCount{ get; set; }
    public int BeforeCount { get; set; }
    public int AfterCount { get; set; }
    public long totalTimeElapsed { get; private set; } = 0;

    public List<PassedTestMethodInfo> Passed { get; private set; }
    public List<FailedTestMethodInfo> Failed { get; private set; }
    public List<SkippedTestMethodInfo> Skipped { get; private set; }
    
    public TestClassInfo(Type t)
    {
        ClassName = t.Name;
        Passed = new();
        Failed = new();
        Skipped = new();
    }

    /// <summary>
    /// Adds passed method test log to class instance
    /// </summary>
    /// <param name="method"></param>
    public void AddPassed(PassedTestMethodInfo method)
    {
        Passed.Add(method);
        totalTimeElapsed += method.TimeElapsed;
    }

    /// <summary>
    /// Adds failed method test log to class instance
    /// </summary>
    /// <param name="method"></param>

    public void AddFailed(FailedTestMethodInfo method)
    {
        Failed.Add(method);
        totalTimeElapsed += method.TimeElapsed;

    }

    /// <summary>
    /// Adds skipped method test log to class instance
    /// </summary>
    /// <param name="method"></param>

    public void AddSkipped(SkippedTestMethodInfo method)
    {
        Skipped.Add(method);
        totalTimeElapsed += method.TimeElapsed;
    }

}


/// <summary>
/// Logs test result for assembly
/// </summary>
public class BuildInfo
{
    public List<TestClassInfo> TestClasses { get; private set; }
    public string BuildName { get; private set; } = "";

    public BuildInfo(Assembly asm)
    {
        BuildName = asm.GetName().Name;
        TestClasses = new();
    }

    public void AddClassInfo(TestClassInfo cls)
    {
        TestClasses.Add(cls);
    }
}