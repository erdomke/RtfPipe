using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RtfPipe.Tests
{
  [TestClass]
  public class GitHubIssues
  {
    [TestMethod]
    public void Issue14()
    {
      TestConvert(@"{\rtf1\ansi\deff0
{\colortbl;\red0\green0\blue0;\red255\green0\blue0;}
{\f0\cf0 Kartl\u228\'e4gga hur }{\b\f0\cf0 medarbetarna}{\f0\cf0 upplever sin h\u228\'e4lsa, arbetsmilj\u246\'f6 och livsstil.}
}", "<div style=\"font-size:12pt;\"><p style=\"margin:0;\">Kartlägga hur <strong>medarbetarna</strong>upplever sin hälsa, arbetsmiljö och livsstil.</p></div>");
    }

    [TestMethod]
    public void Issue16()
    {
      TestConvert(@"{\rtf1\ansi\deff0 {\fonttbl {\f0 Courier;}{\f1 ProFontWindows;}}
{\colortbl;\red0\green0\blue0;\red255\green0\blue0;\red255\green255\blue0;}
This line is font 0 which is courier\line
\f1
This line is font 1\line
\f0
This line is font 0 again\line
This line has a \cf2 red \cf1 word\line
\highlight3 while this line has a \cf2 red \cf1 word and is highlighted in yellow\highlight0\line
Finally, back to the default color.\line
}", "<div style=\"font-size:12pt;font-family:Courier;\"><p style=\"margin:0;\">This line is font 0 which is courier<br><span style=\"font-family:ProFontWindows;\">This line is font 1<br></span>This line is font 0 again<br>This line has a <span style=\"color:#FF0000;\">red </span>word<br><span style=\"background:#FFFF00;\">while this line has a <span style=\"color:#FF0000;\">red </span>word and is highlighted in yellow<br></span><span style=\"background:#FFFFFF;\">Finally, back to the default color.<br></span>&nbsp;</p></div>");
    }

    private void TestConvert(string rtf, string html)
    {
      var actual = Rtf.ToHtml(rtf);
      Assert.AreEqual(html, actual);
    }
  }
}
