using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtfPipe.Tests
{
  /// <summary>
  /// Adapted from http://www.pindari.com/
  /// </summary>
  [TestClass()]
  public class PindariExamples
  {
    [TestMethod()]
    public void Pindari_Colors()
    {
      TestConvert(@"{\rtf1\ansi\deff0
{\colortbl;\red0\green0\blue0;\red255\green0\blue0;}
This line is the default color\line
\cf2
This line is red\line
\cf1
This line is the default color
}", @"<div style=""font-size:12pt;""><p style=""margin:0 0 0 0;"">This line is the default color<br><span style=""color:#FF0000;"">This line is red<br></span><span style=""color:#000000;"">This line is the default color</span></p></div>");
    }

    [TestMethod()]
    public void Pindari_Tabs()
    {
      TestConvert(@"{\rtf1\ansi\deff0 {\fonttbl {\f0 Courier;}}
{\colortbl;\red0\green0\blue0;\red255\green0\blue0;}
\tx720\tx1440\tx2880\tx5760
This line is the default color\line
\tab this line has 1 tab\line
\tab\tab this line has 2 tabs\line
\tab\tab\tab this line has 3 tabs\line
\tab\tab\tab\tab this line has 4 tabs\line
\cf2
\tab This line is red and has a tab before it\line
\cf1
\page This line is the default color and the first line on page 2
}", @"<div style=""font-size:12pt;font-family:Courier;""><p style=""margin:0 0 0 0;"">This line is the default color<br><span style=""display:inline-block;width:48px""></span>this line has 1 tab<br><span style=""display:inline-block;width:96px""></span>this line has 2 tabs<br><span style=""display:inline-block;width:192px""></span>this line has 3 tabs<br><span style=""display:inline-block;width:384px""></span>this line has 4 tabs<br><span style=""display:inline-block;width:48px""></span><span style=""color:#FF0000;"">This line is red and has a tab before it<br></span></p></div><div style=""page-break-before:always;font-size:12pt;font-family:Courier;""><p style=""color:#000000;margin:0 0 0 0;"">This line is the default color and the first line on page 2</p></div>");
    }

    private void TestConvert(string rtf, string html)
    {
      var actual = Rtf.ToHtml(rtf);
      Assert.AreEqual(html, actual);
    }
  }
}
