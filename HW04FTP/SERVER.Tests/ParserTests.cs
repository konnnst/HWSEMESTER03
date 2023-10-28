namespace SERVER.Tests;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void ParseTest()
    {
        var correctGetQuery = "get /home/user/PhildCorn/PM-PU.mp4";
        var correctListQuery = "list Games/Minecraft";
        var incorrectArgCount = "get file1 file2";
        var incorrectCommandWord = "delete /boot";

        Assert.IsTrue(Parser.Parse(correctGetQuery));
        Assert.AreEqual(Parser.CommandType, "get");
        Assert.AreEqual(Parser.Path, "/home/user/PhildCorn/PM-PU.mp4");

        Assert.IsTrue(Parser.Parse(correctListQuery));
        Assert.AreEqual(Parser.CommandType, "list");
        Assert.AreEqual(Parser.Path, "Games/Minecraft");

        Assert.IsFalse(Parser.Parse(incorrectArgCount));

        Assert.IsFalse(Parser.Parse(incorrectCommandWord));
    }   
}