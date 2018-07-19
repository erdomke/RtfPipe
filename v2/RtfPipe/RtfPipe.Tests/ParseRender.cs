using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RtfPipe.Tests
{
  [TestClass]
  public class ParseRender
  {
    [TestMethod]
    public void TestMethod1()
    {
      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RtfPipe.Tests.Files.Test01.rtf"))
      {
        var tokenizer = new Tokenizer(stream);
        var tokens = tokenizer.Tokens().ToList();
        Assert.Fail();
      }
    }

    [TestMethod]
    public void StreamReader()
    {
      var expected = default(string);
      var actual = default(string);

      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RtfPipe.Tests.Files.Test01.rtf"))
      using (var reader = new StreamReader(stream))
      {
        expected = reader.ReadToEnd();
      }

      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RtfPipe.Tests.Files.Test01.rtf"))
      using (var reader = new RtfStreamReader(stream))
      {
        actual = reader.ReadToEnd();
      }

      Assert.AreEqual(expected, actual);
    }
  }
}
