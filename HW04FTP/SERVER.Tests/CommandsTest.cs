using System.IO;
using System;

namespace SERVER.Tests;

[TestClass]
public class CommandsTests
{
    [TestMethod]
    public void GetTest()
    {
        var invalidPath = "/a/b/c/d/e/f/g.txt";
        var correctPath = "../../../../SERVER/Parser.cs";

        var invalidResponse = Commands.Get(invalidPath);
        var correctResponse = Commands.Get(correctPath);

        var givenSize = Convert.ToInt32(correctResponse.Split()[0]);
        var realSize = correctResponse.Split().Count<string>() - 1;

    
        Assert.AreEqual(invalidResponse, "-1");
        Assert.AreEqual(givenSize, realSize);
    }

    private bool IsListResponseCorrect(string response)
    {
        var objectList = response.Split();
        var givenObjectCount = Convert.ToInt32(objectList[0]);
        var realObjectCount = (objectList.Count<string>() - 1) / 2;

        if (givenObjectCount != realObjectCount)
        {
            return false;
        }

        for (var i = 0; i < realObjectCount; ++i)
        {
            Console.WriteLine(objectList[1 + i * 2]);
            if (objectList[1 + i * 2] != "d" && objectList[1 + i * 2] != "f")
            {
                return false;
            }
        }

        return true;
    }

    [TestMethod]
    public void ListTest()
    {
        var invalidPath = "/a/b/c/d/e/f/g.txt";
        var correctPath = "../../../../SERVER";


        var invalidResponse = Commands.List(invalidPath);
        var correctResponse = Commands.List(correctPath);

        Assert.AreEqual(invalidResponse, "-1");
        Assert.IsTrue(IsListResponseCorrect(correctResponse));
    }
}
