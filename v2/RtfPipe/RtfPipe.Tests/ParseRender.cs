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
    public void Prototype()
    {
      TestParse("RtfPipe.Tests.Files.RtfParserTest_4");
    }

    [TestMethod]
    public void RtfToHtml()
    {
      TestConvert("RtfPipe.Tests.Files.Test01");
      TestConvert("RtfPipe.Tests.Files.minimal");
      TestConvert("RtfPipe.Tests.Files.RtfParserTest_0");
      TestConvert("RtfPipe.Tests.Files.RtfParserTest_1");
      TestConvert("RtfPipe.Tests.Files.RtfParserTest_2");
      TestConvert("RtfPipe.Tests.Files.RtfParserTest_3");
    }

    private void TestParse(string path)
    {
      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path + ".rtf"))
      {
        var actual = Rtf.ToHtml(stream);
        Assert.Fail();
      }
    }

    private void TestConvert(string path)
    {
      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path + ".rtf"))
      using (var expectedReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(path + ".html")))
      {
        var actual = Rtf.ToHtml(stream);
        var expected = expectedReader.ReadToEnd();
        Assert.AreEqual(expected, actual);
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
