namespace Unit;

/// <summary>
/// Class for asserting statements
/// </summary>
public static class Assert
{
    /// <summary>
    /// Check if statement is true
    /// </summary>
    /// <param name="statement">Statement to check</param>
    /// <exception cref="Exception">Thrown if statement is false</exception>
    public static void IsTrue(bool statement)
    {
        if (!statement)
        {
            throw new Exception("ACHTUNG! Assert.IsTrue failed!");
        }
    }

    /// <summary>
    /// Check if statement is false
    /// </summary>
    /// <param name="statement">Statement to check</param>
    /// <exception cref="Exception">Thrown if statement is true</exception>
    public static void IsFalse(bool statement)
    {
        if (statement)
        {
            throw new Exception("ACHTUNG! Assert.IsFalse failed");
        }
    }

    /// <summary>
    /// Check if two objects are equal
    /// </summary>
    /// <param name="a">First object</param>
    /// <param name="b">Second object</param>
    /// <exception cref="Exception">Thrown if objects are not equal</exception>
    public static void AreEqual(object a, object b)
    {
        if (!a.Equals(b))
        {
            throw new Exception($"ACHTUNG! Assert.AreEqual failed ({a}, {b} are not equal)");
        }
    }

    /// <summary>
    /// Check if two objects are not equal
    /// </summary>
    /// <param name="a">First object</param>
    /// <param name="b">Second object</param>
    /// <exception cref="Exception">Thrown if objects are equal</exception>
    public static void AreNotEqual(object a, object b)
    {
        if (a.Equals(b))
        {
            throw new Exception($"ACHTUNG! Assert.AreNotEqual failed ({a}, {b} are equal)");
        }
    }

    /// <summary>
    /// Check if object is null
    /// </summary>
    /// <param name="a">Object</param>
    /// <exception cref="Exception">Thrown if object is not null</exception>
    public static void IsNull(object a)
    {
        if (a != null)
        {
            throw new Exception($"ACHTUNG! Assert.IsNull failed ({a} is not null)");
        }
    }

    /// <summary>
    /// Check if object is not null
    /// </summary>
    /// <param name="a">Object</param>
    /// <exception cref="Exception">Thrown if object is null</exception>
    public static void IsNotNull(object a)
    {
        if (a == null)
        {
            throw new Exception($"ACHTUNG! Assert.IsNotNull failed ({a} is null)");
        }
    }

    /// <summary>
    /// Fails always
    /// </summary>
    /// <param name="message">Custom message to add in exception message</param>
    /// <exception cref="Exception">Thrown always. Contains custom message</exception>
    public static void Fail(string message)
    {
        if (message.Count() != 0)
        {
            message = $" ({message})";
        }
        throw new Exception($"ACHTUNG! Failed{message}");
    }

    /// <summary>
    /// Fails always
    /// </summary>
    public static void Fail()
    {
        Fail("");
    }
}