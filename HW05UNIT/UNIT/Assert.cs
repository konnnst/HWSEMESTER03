namespace Unit;

public static class Assert
{
    public static void IsTrue(bool condition)
    {
        if (!condition)
        {
            throw new Exception("ACHTUNG! Assert.IsTrue failed!");
        }
    }

    public static void IsFalse(bool condition)
    {
        if (condition)
        {
            throw new Exception("ACHTUNG! Assert.IsFalse failed");
        }
    }

    public static void AreEqual(object a, object b)
    {
        if (!a.Equals(b))
        {
            throw new Exception($"ACHTUNG! Assert.AreEqual failed ({a}, {b} are not equal)");
        }
    }

    public static void AreNotEqual(object a, object b)
    {
        if (!a.Equals(b))
        {
            throw new Exception($"ACHTUNG! Assert.AreNotEqual failed ({a}, {b} are equal)");
        }
    }

    public static void IsNull(object a)
    {
        if (a == null)
        {
            throw new Exception($"ACHTUNG! Assert.IsNull failed");
        }
    }

    public static void IsNotNull(object a)
    {
        if (a != null)
        {
            throw new Exception($"ACHTUNG! Assert.IsNotNull failed");
        }
    }

    public static void Fail(string message)
    {
        if (message.Count() != 0)
        {
            message = $" ({message})";
        }
        throw new Exception($"ACHTUNG! Failed{message}");
    }

    public static void Fail()
    {
        Fail("");
    }
}