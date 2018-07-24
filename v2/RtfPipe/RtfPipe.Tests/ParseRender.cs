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
      TestParse("RtfPipe.Tests.Files.Headings");
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

    [TestMethod]
    public void EncapsulatedHtml()
    {
      // FROM: https://msdn.microsoft.com/en-us/library/ee204432(v=exchg.80).aspx
      var rtf = @"{\rtf1\ANSI\ansicpg1251\fromhtml1 \deff0
{\fonttbl {\f0\fmodern Courier New;}{\f1\fswiss Arial;}{\f2\fswiss\fcharset0 Arial;}}
{\colortbl\red0\green0\blue0;\red0\green0\blue255;} 
{\*\htmltag64}
\uc1\pard\plain\deftab360 \f0\fs24
{\*\htmltag <HTML><head>\par
<style>\par
<!--\par
 /* Style Definitions */\par
 p.MsoNormal, li.MsoNormal \{font-family:Arial;\}\par
-->\par
</style>\par
\tab <!-- This is a HTML comment.\par
There is a horizontal tab (%x09) character before the comment, \par
and some new lines inside the comment. -->\par
</head>\par
<body>\par
<p\par
class=""MsoNormal"">}
{\htmlrtf \f1 \htmlrtf0 Note the line break inside a P tag. {\*\htmltag <b>}{\htmlrtf \b \htmlrtf0 This is a bold text{\*\htmltag </b>}} \htmlrtf\par\htmlrtf0}
\htmlrtf \par \htmlrtf0
{\*\htmltag </p>\par
<p class=""MsoNormal"">\par}
{\htmlrtf \f1 \htmlrtf0 This is a normal text with a character references:
{\*\htmltag &nbsp;}\htmlrtf \'a0\htmlrtf0  {\*\htmltag &lt;}\htmlrtf <\htmlrtf0  {\*\htmltag &uml;}\htmlrtf {\f2\'a8}\htmlrtf0{\*\htmltag <br>\par}\htmlrtf\line\htmlrtf0
characters which have special meaning in RTF: \{\}\\{\*\htmltag <br>\par}\htmlrtf\line\htmlrtf0\htmlrtf\par\htmlrtf0}
{\*\htmltag </p>\par
<ol>\par
    <li class=""MsoNormal"">}{\htmlrtf {{\*\pn\pnlvlbody\pndec\pnstart1\pnindent360{\pntxta.}}\li360\fi-360{\pntext 1.\tab} \f1 \htmlrtf0 This is a list item}\htmlrtf\par\htmlrtf0}
{\*\htmltag \par
</ol>\par
</body>\par
</HTML>\par }}";
      var html = Rtf.ToHtml(rtf);
      var expected = @" <HTML><head>
<style>
<!--
 /* Style Definitions */
 p.MsoNormal, li.MsoNormal {font-family:Arial;}
-->
</style>
<!-- This is a HTML comment.
There is a horizontal tab (%x09) character before the comment, 
and some new lines inside the comment. -->
</head>
<body>
<p
class=""MsoNormal"">Note the line break inside a P tag. <b>This is a bold text</b> </p>
<p class=""MsoNormal"">
This is a normal text with a character references:&nbsp; &lt; &uml;<br>
characters which have special meaning in RTF: {}\<br>
</p>
<ol>
    <li class=""MsoNormal"">This is a list item
</ol>
</body>
</HTML>
";
      Assert.AreEqual(expected, html);
    }
  }
}
