namespace Unit;

[AttributeUsage(AttributeTargets.Method)]
public class Test : Attribute
{
    public string[] Expected { get; } = {};
    public string Ignore { get; } = "NORMAL";

    public Test() {}

    public Test(string ignore)
    {
        Ignore = ignore;
    }

    public Test(string[] expected)
    {
        Expected = expected;
    }

    public Test(string[] expected, string ignore)
    {
        Expected = expected;
        Ignore = ignore;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class BeforeClass : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public class AfterClass : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public class Before : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public class After : Attribute
{
}