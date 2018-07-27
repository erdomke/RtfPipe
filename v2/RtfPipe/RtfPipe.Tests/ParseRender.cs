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
    //[TestMethod]
    //public void Prototype()
    //{
    //  TestParse("RtfPipe.Tests.Files.Hyperlink");
    //}

    /// <summary>
    /// Many examples are taken from https://github.com/paulhtremblay/rtf2xml/tree/master/test/test_files/good
    /// </summary>
    [TestMethod]
    public void RtfToHtml()
    {
      TestConvert("RtfPipe.Tests.Files.rtf2xml.bullet_list");
      TestConvert("RtfPipe.Tests.Files.rtf2xml.char_upper_ranges");
      TestConvert("RtfPipe.Tests.Files.rtf2xml.color");
      TestConvert("RtfPipe.Tests.Files.rtf2xml.diff_types_border");
      TestConvert("RtfPipe.Tests.Files.rtf2xml.escaped_text");
      TestConvert("RtfPipe.Tests.Files.rtf2xml.heading_with_section");
      TestConvert("RtfPipe.Tests.Files.rtf2xml.headings_mixed");
      TestConvert("RtfPipe.Tests.Files.rtf2xml.table_simple");
      //TestConvert("RtfPipe.Tests.Files.Picture");
      TestConvert("RtfPipe.Tests.Files.Test01");
      TestConvert("RtfPipe.Tests.Files.minimal");
      TestConvert("RtfPipe.Tests.Files.RtfParserTest_0");
      TestConvert("RtfPipe.Tests.Files.RtfParserTest_1");
      TestConvert("RtfPipe.Tests.Files.RtfParserTest_2");
      TestConvert("RtfPipe.Tests.Files.RtfParserTest_3");
      TestConvert("RtfPipe.Tests.Files.rtf2xml.Hyperlink");
      TestConvert("RtfPipe.Tests.Files.rtf2xml.caps_mixed");
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

    [TestMethod]
    public void ImageSize()
    {
      const string rtf = @"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}}
{\*\generator Riched20 10.0.14393}\viewkind4\uc1 
\pard\sa200\sl240\slmult1\f0\fs22\lang9{\pict{\*\picprop}\wmetafile8\picw1323\pich265\picwgoal750\pichgoal150 
010009000003f60000000000cd00000000000400000003010800050000000b0200000000050000
000c020a003200030000001e0004000000070104000400000007010400cd000000410b2000cc00
0a003200000000000a0032000000000028000000320000000a0000000100040000000000000000
000000000000000000000000000000000000000000ffffff003300ff000033ff00000000000000
000000000000000000000000000000000000000000000000000000000000000000000000000000
000000222222222222222222222222222222222222222222222222220202022222222222222222
222222222222222222222222222222222202020222222222222222222222222222222222222222
222222222222020202222222222222222222222222222222222222222222222222220202022222
222222222222222222222222222222222222222222222202020222222222222222222222222222
222222222222222222222222020202222222222222222222222222222222222222222222222222
220202022222222222222222222222222222222222222222222222222202020222222222222222
222222222222222222222222222222222222020202222222222222222222222222222222222222
22222222222232020202040000002701ffff030000000000
}\par
\par
\par
{\pict{\*\picprop}\wmetafile8\picw1323\pich265\picwgoal750\pichgoal150 
0100090000037600000000004d00000000000400000003010800050000000b0200000000050000
000c020a003200030000001e00040000000701040004000000070104004d000000410b2000cc00
0a003200000000000a0032000000000028000000320000000a0000000100010000000000000000
000000000000000000000000000000000000000000ffffff00ffffffffffffc001ffffffffffff
c001ffffffffffffc001ffffffffffffc001ffffffffffffc001ffffffffffffc001ffffffffff
ffc001ffffffffffffc001ffffffffffffc001ffffffffffffc001040000002701ffff03000000
0000
}\par

\pard\sa200\sl276\slmult1\par
}";
      var html = Rtf.ToHtml(rtf);
      var expected = "<div style=\"font-size:12pt;font-family:Calibri;\"><p style=\"font-size:11pt;margin:0 0 13.3px 0;\"><img width=\"50\" height=\"10\" src=\"data:windows/metafile;base64,AQAJAAAD9gAAAAAAzQAAAAAABAAAAAMBCAAFAAAACwIAAAAABQAAAAwCCgAyAAMAAAAeAAQAAAAHAQQABAAAAAcBBADNAAAAQQsgAMwACgAyAAAAAAAKADIAAAAAACgAAAAyAAAACgAAAAEABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////ADMA/wAAM/8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIgICAiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiICAgIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiAgICIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIgICAiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiICAgIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiAgICIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIgICAiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiICAgIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiAgICIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiMgICAgQAAAAnAf//AwAAAAAA\"></p><p style=\"font-size:11pt;margin:0 0 13.3px 0;\"><br></p><p style=\"font-size:11pt;margin:0 0 13.3px 0;\"><br></p><p style=\"font-size:11pt;margin:0 0 13.3px 0;\"><img width=\"50\" height=\"10\" src=\"data:windows/metafile;base64,AQAJAAADdgAAAAAATQAAAAAABAAAAAMBCAAFAAAACwIAAAAABQAAAAwCCgAyAAMAAAAeAAQAAAAHAQQABAAAAAcBBABNAAAAQQsgAMwACgAyAAAAAAAKADIAAAAAACgAAAAyAAAACgAAAAEAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////AP///////8AB////////wAH////////AAf///////8AB////////wAH////////AAf///////8AB////////wAH////////AAf///////8ABBAAAACcB//8DAAAAAAA=\"></p><p style=\"font-size:11pt;margin:0 0 13.3px 0;\"><br></p></div>";
      Assert.AreEqual(expected, html);
    }
  }
}
