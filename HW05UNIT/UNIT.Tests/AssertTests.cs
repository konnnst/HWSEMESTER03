namespace Unit.Tests;

[TestClass]
public class AssertTests
{
    [TestMethod]
    [ExpectedException(typeof(Exception), "ACHTUNG! Assert.IsTrue failed!")]
    public void IsTrueFailTest()
    {
        Assert.IsTrue(false);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "ACHTUNG! Assert.IsFalse failed!")]
    public void IsFalseFailTest()
    {
        Assert.IsFalse(true);
    }

    [TestMethod]
    public void IsTrueOkTest()
    {
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void IsFalseOkTest()
    {
        Assert.IsFalse(false);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "ACHTUNG! Assert.AreEqual failed (1, 2 are not equal)")]
    public void AreEqualFailTest()
    {
        Assert.AreEqual(1, 2);
    }

    [TestMethod]
    public void AreEqualOkTest()
    {
        Assert.AreEqual(1, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "ACHTUNG! Assert.AreNotEqual failed (1, 1 are equal)")]
    public void AreNotEqualFailTest()
    {
        Assert.AreNotEqual(1, 1);
    }

    [TestMethod]
    public void AreNotEqualOkTest()
    {
        Assert.AreNotEqual(1, 2);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "ACHTUNG! Assert.IsNull failed (1 is not null)")]
    public void IsNullFailTest()
    {
        Assert.IsNull(1);
    }

    [TestMethod]
    public void IsNullOkTest()
    {
        Assert.IsNull(null);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "ACHTUNG! Assert.IsNotNull failed (null is null)")]
    public void IsNotNullFailTest()
    {
        Assert.IsNotNull(null);
    }

    [TestMethod]
    public void IsNotNullOkTest()
    {
        Assert.IsNotNull(1);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Achtung! Failed (oblom)")]
    public void FailMessageTest()
    {
        Assert.Fail("oblom");
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Achtung! Failed")]
    public void FailTest()
    {
        Assert.Fail();
    }
}