using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RtfPipe;

namespace RtfPipe.Tests
{
  [TestClass]
  public class RoundTrip
  {
    [TestMethod]
    public void RoundTrip_Formatting()
    {
      const string rtf = @"{\rtf1\ansi\ansicpg1252\uc1\deff1{\fonttbl
{\f0\fswiss\fcharset0\fprq2 Arial;}
{\f1\fswiss\fcharset0\fprq2 Verdana;}
{\f2\froman\fcharset2\fprq2 Symbol;}}
{\colortbl;\red255\green0\blue0;\red0\green255\blue0;\red0\green0\blue255;}
{\stylesheet{\s0\itap0\nowidctlpar\f0\fs24 [Normal];}}
{\*\generator TX_RTF32 14.0.520.501;}
\sectd
\pard\itap0\nowidctlpar\plain\f1\fs36
{Hellou RTF Wörld\par
\f0\fs24\par
with some symbols: ""{\f2\cf1\cb0\chcbpat3\i abc}""\par
\par
and some \b bold\b0, \i italic\i0, \ul underlined\ul0  and \strike strikethrough\strike0  text\par
\par
some nested styles: {\b\cf1 bold}, {\i\cf2 italic}, {\ul\cf3 underlined}, normal\par
\par
some combined styles: {\b bold+\i italic+\ul underlined} vs. normal\par
\par
and further: {\b bold, {\i bold+italic}, \ul bold+underlined{\plain, normal}}\par
\par
different ways: [A] == [\'41] == [\u65A] == {\uc0[\u65]}\par
more unicode: Unicode: [\u915G] - ANSI-fallback: [G]\par
same but different: ""{\upr{[G]}{\*\ud{[\uc0\u915]}}}""\par
\par
something to ignore: {\*\unsupportedtag {\b should not appear}{\i this neither}}{\b visible}.
\par }
}";
      const string html = @"<!DOCTYPE html ><html><head><meta http-equiv=""content-type"" content=""text/html; charset=UTF-8"" /></head><body><p><span style=""font-family:Verdana;font-size:18pt"">Hellou RTF Wörld</span></p><p>&nbsp;</p><p><span style=""font-family:Arial;font-size:12pt"">with some symbols: ""</span><i><span style=""color:#ff0000;background-color:#0000ff;font-family:Symbol;font-size:12pt"">abc</span></i><span style=""font-family:Arial;font-size:12pt"">""</span></p><p>&nbsp;</p><p><span style=""font-family:Arial;font-size:12pt"">and some </span><b><span style=""font-family:Arial;font-size:12pt"">bold</span></b><span style=""font-family:Arial;font-size:12pt"">, </span><i><span style=""font-family:Arial;font-size:12pt"">italic</span></i><span style=""font-family:Arial;font-size:12pt"">, </span><u><span style=""font-family:Arial;font-size:12pt"">underlined</span></u><span style=""font-family:Arial;font-size:12pt""> and </span><s><span style=""font-family:Arial;font-size:12pt"">strikethrough</span></s><span style=""font-family:Arial;font-size:12pt""> text</span></p><p>&nbsp;</p><p><span style=""font-family:Arial;font-size:12pt"">some nested styles: </span><b><span style=""color:#ff0000;font-family:Arial;font-size:12pt"">bold</span></b><span style=""font-family:Arial;font-size:12pt"">, </span><i><span style=""color:#00ff00;font-family:Arial;font-size:12pt"">italic</span></i><span style=""font-family:Arial;font-size:12pt"">, </span><u><span style=""color:#0000ff;font-family:Arial;font-size:12pt"">underlined</span></u><span style=""font-family:Arial;font-size:12pt"">, normal</span></p><p>&nbsp;</p><p><span style=""font-family:Arial;font-size:12pt"">some combined styles: </span><b><span style=""font-family:Arial;font-size:12pt"">bold+</span></b><b><i><span style=""font-family:Arial;font-size:12pt"">italic+</span></i></b><b><i><u><span style=""font-family:Arial;font-size:12pt"">underlined</span></u></i></b><span style=""font-family:Arial;font-size:12pt""> vs. normal</span></p><p>&nbsp;</p><p><span style=""font-family:Arial;font-size:12pt"">and further: </span><b><span style=""font-family:Arial;font-size:12pt"">bold, </span></b><b><i><span style=""font-family:Arial;font-size:12pt"">bold+italic</span></i></b><b><span style=""font-family:Arial;font-size:12pt"">, </span></b><b><u><span style=""font-family:Arial;font-size:12pt"">bold+underlined</span></u></b><span style=""font-family:Arial;font-size:12pt"">, normal</span></p><p>&nbsp;</p><p><span style=""font-family:Arial;font-size:12pt"">different ways: [A] == [A] == [A] == [A]</span></p><p><span style=""font-family:Arial;font-size:12pt"">more unicode: Unicode: [Γ] - ANSI-fallback: [G]</span></p><p><span style=""font-family:Arial;font-size:12pt"">same but different: ""[Γ]""</span></p><p>&nbsp;</p><p><span style=""font-family:Arial;font-size:12pt"">something to ignore: </span><b><span style=""font-family:Arial;font-size:12pt"">visible</span></b><span style=""font-family:Arial;font-size:12pt"">.</span></p></body></html>";
      var output = Rtf.ToHtml(rtf);
      Assert.AreEqual(html, output);
    }
  }
}
