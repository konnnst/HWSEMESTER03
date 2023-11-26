using Unit;
using Proj;

namespace Tests;

public class OperationsTests
{
    [Test("матмех")]
    public void PlusTest()
    {
        Console.WriteLine("Plus test is running");
    }

    [Test(true, "лучше всех")]
    public void MinusTest()
    {
        Console.WriteLine(1.Equals(1));
        Console.WriteLine("Minus test is running");

        Assert.Fail();
    }

    [Test(false)]
    public void MultTest()
    {
        Console.WriteLine("Mult test is running");
    }

    [Test()]
    public void DivTest()
    {
        Console.WriteLine("Div test is running");
    }

}
