namespace Unit;

[AttributeUsage(AttributeTargets.Method)]
public class Test : Attribute
{
    public bool Expected { get; } = false;
    public string Ignore { get; } = "NORMAL";

    public Test() {}

    public Test(string ignore)
    {
        Ignore = ignore;
    }

    public Test(bool expected)
    {
        Expected = expected;
    }

    public Test(bool expected, string ignore)
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