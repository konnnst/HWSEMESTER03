namespace SERVER.Tests;

[TestClass]
public class ExecutorTests
{
    [TestMethod]
    public async Task RespondGetCommandTest()
    {
        string[] getCommand = { "get", "../../../../SERVER/Parser.cs" };

        Assert.AreNotEqual(await Executor.RespondCommand(getCommand[0], getCommand[1]), "-1");
    }

    [TestMethod]
    public async Task RespondListCommandTest()
    {
        string[] listCommand = { "list", "." };

        Assert.AreNotEqual(await Executor.RespondCommand(listCommand[0], listCommand[1]), "-1");
    }

    [TestMethod]
    public async Task RespondIncorrectCommandWordTest()
    {
        string[] incorrectCommandWord = { "rm -rfd", "/" };
        Assert.AreEqual(await Executor.RespondCommand(incorrectCommandWord[0],
        incorrectCommandWord[1]), "Incorrect command word!");
    }
}