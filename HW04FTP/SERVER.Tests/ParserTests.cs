namespace SERVER.Tests;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void ParseCorrectGetTest()
    {
        var correctGetQuery = "get /home/user/PhildCorn/PM-PU.mp4";

        Assert.IsTrue(Parser.Parse(correctGetQuery));
        Assert.AreEqual(Parser.CommandType, "get");
        Assert.AreEqual(Parser.Path, "/home/user/PhildCorn/PM-PU.mp4");
    }   

    [TestMethod]
    public void ParseCorrectListTest()
    {
        var correctListQuery = "list Games/Minecraft";

        Assert.IsTrue(Parser.Parse(correctListQuery));
        Assert.AreEqual(Parser.CommandType, "list");
        Assert.AreEqual(Parser.Path, "Games/Minecraft");
    }

    [TestMethod]
    public void ParseIncorrectArgCountTest()
    {
        var incorrectArgCount = "get file1 file2";

        Assert.IsFalse(Parser.Parse(incorrectArgCount));
    }

    [TestMethod]
    public void ParseIncorrectCommandWordTest()
    {
        var incorrectCommandWord = "delete /boot";

        Assert.IsFalse(Parser.Parse(incorrectCommandWord));
    }
}