namespace SERVER.Tests;

[TestClass]
public class ExecutorTests
{
    [TestMethod]
    public void RespondCommandTest()
    {
        string[] incorrectCommandWord = { "rm -rfd", "/" };
        string[] getCommand = { "get", "../../../../SERVER/Parser.cs" };
        string[] listCommand = { "list", "." };

        Assert.AreEqual(Executor.RespondCommand(incorrectCommandWord[0],
        incorrectCommandWord[1]), "ACHTUNG! ACHTUNG! INCORRECT COMMAND WORD!");
        Assert.AreNotEqual(Executor.RespondCommand(getCommand[0], getCommand[1]), "-1");
        Assert.AreNotEqual(Executor.RespondCommand(listCommand[0], listCommand[1]), "-1");
    }
}