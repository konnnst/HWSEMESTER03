namespace UNIT.Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        Assert.IsTrue(4 == 4);
    }

    [TestMethod]
    public void TestMethod2()
    {
        Assert.AreEqual(1, 1);
        Assert.Fail();
    }
}