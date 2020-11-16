using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtfPipe.Tests
{
  /// <summary>
  /// Examples taken from https://www.safaribooksonline.com/library/view/rtf-pocket-guide/9781449302047/ch01.html
  /// </summary>
  [TestClass]
  public class OReillyExamples
  {
    [TestMethod()]
    public void HelloWorld()
    {
      TestConvert(@"{\rtf1\ansi\deff0 {\fonttbl {\f0 Times New Roman;}}
\f0\fs60 Hello, World!
}", @"<div style=""font-size:12pt;font-family:&quot;Times New Roman&quot;;""><p style=""font-size:30pt;margin:0;"">Hello, World!</p></div>");
    }

    [TestMethod()]
    public void HelloWorld_Centered()
    {
      TestConvert(@"{\rtf1\ansi\deff0 {\fonttbl {\f0 Monotype Corsiva;}}
\qc\f0\fs120\i\b Hello,\line World!
}", @"<div style=""font-size:12pt;font-family:&quot;Monotype Corsiva&quot;;""><p style=""text-align:center;font-size:60pt;margin:0;""><strong><em>Hello,<br>World!</em></strong></p></div>");
    }

    private void TestConvert(string rtf, string html)
    {
      var actual = Rtf.ToHtml(rtf);
      ParseRender.AssertEqual(html, actual);
    }
  }
}
