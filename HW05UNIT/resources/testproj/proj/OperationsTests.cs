using Unit;
using Proj;
using System.Threading;

namespace Tests;

[UnitTestClass]
public class OperationsTests
{
    [Test()]
    public void PlusTest()
    {
        Thread.Sleep(400);
        throw new Exception("a");
        Console.WriteLine("Plus test is running");
    }

    [Test(new string[] {"ACHTUNG! Failed"})]
    public void MinusTest()
    {
        Console.WriteLine(1.Equals(1));
        Thread.Sleep(1050);
        Console.WriteLine("Minus test is running");

        Assert.Fail();
    }

    [Test("ololo")]
    public void MultTest()
    {
        Console.WriteLine("Mult test is running");
        Assert.Fail();
    }

    [Test()]
    public void DivTest()
    {
        Console.WriteLine("Div test is running");
    }

}
