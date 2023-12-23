namespace Unit;

public class CorrectTestMethodInfo : TestMethodInfo
{
    public CorrectTestMethodInfo(string name, long timeElapsed)
    {
        Name = name;
        TimeElapsed = timeElapsed;
    }
}

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

public class TestClassInfo
{
    public string ClassName { get; private set; } = "";
    public int TestMethodsCount { get; set; }
    public int BeforeClassCount { get; set; }
    public int AfterClassCount{ get; set; }
    public int BeforeCount { get; set; }
    public int AfterCount { get; set; }
    public long totalTimeElapsed { get; private set; } = 0;

    public List<CorrectTestMethodInfo> Correct { get; private set; }
    public List<FailedTestMethodInfo> Failed { get; private set; }
    public List<SkippedTestMethodInfo> Skipped { get; private set; }
    
    public TestClassInfo(Type t)
    {
        ClassName = t.Name;
        Correct = new();
        Failed = new();
        Skipped = new();
    }

    public void AddCorrect(CorrectTestMethodInfo method)
    {
        Correct.Add(method);
        totalTimeElapsed += method.TimeElapsed;
    }

    public void AddFailed(FailedTestMethodInfo method)
    {
        Failed.Add(method);
        totalTimeElapsed += method.TimeElapsed;

    }

    public void AddSkipped(SkippedTestMethodInfo method)
    {
        Skipped.Add(method);
        totalTimeElapsed += method.TimeElapsed;
    }

}

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