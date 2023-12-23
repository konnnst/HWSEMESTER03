namespace Unit;


/// <summary>
/// Marks test methods
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class Test : Attribute
{
    public string[] Expected { get; } = {};
    public string Ignore { get; } = "NORMAL";

    public Test() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ignore">Skips marked test method if not "NORMAL" value</param>
    public Test(string ignore)
    {
        Ignore = ignore;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expected">Expects exceptions, with method noted in expected array</param>
    public Test(string[] expected)
    {
        Expected = expected;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expected">Expects exceptions, with method noted in expected array</param>
    /// <param name="ignore">Skips marked test method if not "NORMAL" value</param>
    public Test(string[] expected, string ignore)
    {
        Expected = expected;
        Ignore = ignore;
    }
}


/// <summary>
/// Marks method running before all test methods
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class BeforeClass : Attribute
{
}


/// <summary>
/// Marks method running after all test methods
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AfterClass : Attribute
{
}

/// <summary>
/// Marks method running before each test method
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class Before : Attribute
{
}

/// <summary>
/// Marks method running after each test method
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class After : Attribute
{
}

/// <summary>
/// Marks class as TestClass, where test methods will be searched
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class UnitTestClass : Attribute
{
}