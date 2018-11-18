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

    private void TestConvert(string rtf, string html)
    {
      var actual = Rtf.ToHtml(rtf);
      Assert.AreEqual(html, actual);
    }
  }
}
