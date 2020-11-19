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
}", "<div style=\"font-size:12pt;\"><p style=\"margin:0;\">This line is the default color<br><span style=\"color:#FF0000;\">This line is red<br></span>This line is the default color</p></div>");
    }

    [TestMethod()]
    public void Pindari_DefaultTabs()
    {
      TestConvert(@"{\rtf1\ansi\deff0 {\fonttbl {\f0 Courier;}}
{\colortbl;\red0\green0\blue0;\red255\green0\blue0;}
This line is the default color\line
\cf2
\tab This line is red and has a tab before it\line
\cf1
\page This line is the default color and the first line on page 2
}
", "<div style=\"font-size:12pt;font-family:Courier;\"><div><p style=\"margin:0;\">This line is the default color<br><span style=\"display:inline-block;width:48px\"></span><span style=\"color:#FF0000;\">This line is red and has a tab before it<br>&nbsp;</span></p></div><div style=\"page-break-before:always;\"><p style=\"margin:0;\">This line is the default color and the first line on page 2</p></div></div>");
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
}", "<div style=\"font-size:12pt;font-family:Courier;\"><div><p style=\"margin:0;\">This line is the default color<br><span style=\"display:inline-block;width:48px\"></span>this line has 1 tab<br><span style=\"display:inline-block;width:96px\"></span>this line has 2 tabs<br><span style=\"display:inline-block;width:192px\"></span>this line has 3 tabs<br><span style=\"display:inline-block;width:384px\"></span>this line has 4 tabs<br><span style=\"display:inline-block;width:48px\"></span><span style=\"color:#FF0000;\">This line is red and has a tab before it<br>&nbsp;</span></p></div><div style=\"page-break-before:always;\"><p style=\"margin:0;\">This line is the default color and the first line on page 2</p></div></div>");
    }

    [TestMethod()]
    public void Pindari_Formatting()
    {
      TestConvert(@"{\rtf1\ansi\deff0 {\fonttbl {\f0 Courier;}}
{\colortbl;\red0\green0\blue0;\red255\green0\blue0;}
\landscape
\paperw15840\paperh12240\margl720\margr720\margt720\margb720
\tx720\tx1440\tx2880\tx5760
This line is the default color\line
\tab this line has 1 tab\line
\tab\tab this line has 2 tabs\line
\tab\tab\tab this line has 3 tabs\line
\tab\tab\tab\tab this line has 4 tabs\line
\cf2
\tab This line is red and has a tab before it\line
\cf1
\page
\par\pard\tx1440\tx2880
This line is the default color and the first line on page 2\line
\tab\tab This is the second tab on the second line on the second page\line
\page
\par\pard
This is the third page with formatting examples\line
\fs30 First line with 15 point text\line
\fs20 Second line with 10 point test\line
\i Italics on \i0 Italics off\line
\b Bold on \b0 Bold off\line
\scaps Small Caps On \scaps0 Small Caps Off\line
\strike Stike through on \strike0 Strike Off\line
\caps All Capitals On \caps0 All Capitals Off\line
\outl Outline on \outl0 Outline Off\line
\ul Underline on \ul0 Underline Off\line
\uldb Double Underline on \ul0 Double Underline Off\line
\ulth Thick Underline on \ul0 Thick Underline Off\line
\ulw Underline words only on \ul0 Underline words only off\line
\ulwave Wave Underline on \ul0 Wave underline off\line
\uld Dotted Underline on \ul0 Dotted underline off\line
\uldash Dash Underline on \ul0 Dash underline off\line
\uldashd Dot Dash Underline on \ul0 Dot Dash underline off\line
}
", "<div style=\"font-size:12pt;font-family:Courier;\"><div><p style=\"margin:0;\">This line is the default color<br><span style=\"display:inline-block;width:48px\"></span>this line has 1 tab<br><span style=\"display:inline-block;width:96px\"></span>this line has 2 tabs<br><span style=\"display:inline-block;width:192px\"></span>this line has 3 tabs<br><span style=\"display:inline-block;width:384px\"></span>this line has 4 tabs<br><span style=\"display:inline-block;width:48px\"></span><span style=\"color:#FF0000;\">This line is red and has a tab before it<br>&nbsp;</span></p></div><div style=\"page-break-before:always;\"><p style=\"margin:0;\"><br></p><p style=\"margin:0;\">This line is the default color and the first line on page 2<br><span style=\"display:inline-block;width:192px\"></span>This is the second tab on the second line on the second page<br>&nbsp;</p></div><div style=\"page-break-before:always;\"><p style=\"margin:0;\"><br></p><p style=\"font-size:10pt;margin:0;\"><span style=\"font-size:12pt;\">This is the third page with formatting examples<br></span><span style=\"font-size:15pt;\">First line with 15 point text<br></span>Second line with 10 point test<br><em>Italics on </em>Italics off<br><strong>Bold on </strong>Bold off<br><span style=\"font-variant:small-caps;\">Small Caps On </span>Small Caps Off<br><s>Stike through on </s>Strike Off<br><span style=\"text-transform:uppercase;\">All Capitals On </span>All Capitals Off<br><span style=\"text-shadow:-1px -1px 0 #000, 1px -1px 0 #000, -1px 1px 0 #000, 1px 1px 0 #000;color:#fff;\">Outline on </span>Outline Off<br><u>Underline on </u>Underline Off<br><u style=\"text-decoration-style:double;\">Double Underline on </u>Double Underline Off<br><u>Thick Underline on </u>Thick Underline Off<br><u style=\"text-decoration-skip:spaces;\">Underline words only on </u>Underline words only off<br><u style=\"text-decoration-style:wavy;\">Wave Underline on </u>Wave underline off<br><u style=\"text-decoration-style:dotted;\">Dotted Underline on </u>Dotted underline off<br><u style=\"text-decoration-style:dashed;\">Dash Underline on </u>Dash underline off<br><u style=\"text-decoration-style:dashed;\">Dot Dash Underline on </u>Dot Dash underline off<br>&nbsp;</p></div></div>");
    }

    [TestMethod()]
    public void Pindari_Table()
    {
      TestConvert(@"{\rtf1\ansi\deff0
\trowd
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx1000
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx2000
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx3000
cell 1\intbl\cell
lots of text in cell two\intbl\cell
cell 3\intbl\cell
\row
}", "<div style=\"font-size:12pt;\"><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:66.7px;\"><col style=\"width:66.7px;\"><col style=\"width:66.7px;\"></colgroup><tr><td style=\"vertical-align:top;border:1px solid;padding:0;\">cell 1</td><td style=\"vertical-align:top;border:1px solid;padding:0;\">lots of text in cell two</td><td style=\"vertical-align:top;border:1px solid;padding:0;\">cell 3</td></tr></table></div>");
    }

    [TestMethod()]
    public void Pindari_Table_Borders()
    {
      TestConvert(@"{\rtf1\ansi\deff0
Below are the border types\line\par

\trowd\trgaph144
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx5000
Single border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrdot\clbrdrl\brdrdot\clbrdrb\brdrdot\clbrdrr\brdrdot
\cellx5000
Dotted border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrdb\clbrdrl\brdrdb\clbrdrb\brdrdb\clbrdrr\brdrdb
\cellx5000
Double thickness border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrdash\clbrdrl\brdrdash\clbrdrb\brdrdash\clbrdrr\brdrdash
\cellx5000
Dashed border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrdashsm\clbrdrl\brdrdashsm\clbrdrb\brdrdashsm\clbrdrr\brdrdashsm
\cellx5000
Small dashed border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrdashd\clbrdrl\brdrdashd\clbrdrb\brdrdashd\clbrdrr\brdrdashd
\cellx5000
Dot dash border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrdashdd\clbrdrl\brdrdashdd\clbrdrb\brdrdashdd\clbrdrr\brdrdashdd
\cellx5000
Dot dot dash border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrtriple\clbrdrl\brdrtriple\clbrdrb\brdrtriple\clbrdrr\brdrtriple
\cellx5000
Triple border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrtnthlg\clbrdrl\brdrtnthlg\clbrdrb\brdrthtnlg\clbrdrr\brdrthtnlg
\cellx5000
Thick thin border (large)\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrtnthlg\clbrdrl\brdrtnthlg\clbrdrb\brdrthtnlg\clbrdrr\brdrthtnlg
\cellx5000
Thin thick border (large)\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrtnthtnlg\clbrdrl\brdrtnthtnlg\clbrdrb\brdrtnthtnlg\clbrdrr\brdrtnthtnlg
\cellx5000
Thin thick thin border (large)\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrwavy\clbrdrl\brdrwavy\clbrdrb\brdrwavy\clbrdrr\brdrwavy
\cellx5000
Wavy border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrdashdotstr\clbrdrl\brdrdashdotstr\clbrdrb\brdrdashdotstr\clbrdrr\brdrdashdotstr
\cellx5000
Striped border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdremboss\clbrdrl\brdremboss\clbrdrb\brdrengrave\clbrdrr\brdrengrave
\cellx5000
Emboss border\intbl\cell
\row\pard\par

\trowd\trgaph144
\clbrdrt\brdrengrave\clbrdrl\brdrengrave\clbrdrb\brdremboss\clbrdrr\brdremboss
\cellx5000
Engrave border\intbl\cell
\row\pard\par

End of border types.
} ", "<div style=\"font-size:12pt;\"><p style=\"margin:0;\">Below are the border types<br>&nbsp;</p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:1px solid;padding:0 9.6px;\">Single border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:1px dotted;padding:0 9.6px;\">Dotted border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:3px double;padding:0 9.6px;\">Double thickness border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:1px dashed;padding:0 9.6px;\">Dashed border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:1px dashed;padding:0 9.6px;\">Small dashed border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:1px dashed;padding:0 9.6px;\">Dot dash border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:1px dotted;padding:0 9.6px;\">Dot dot dash border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:3px double;padding:0 9.6px;\">Triple border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border-top:3px double;border-right:3px double;border-bottom:3px double;border-left:3px double;padding:0 9.6px;\">Thick thin border (large)</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border-top:3px double;border-right:3px double;border-bottom:3px double;border-left:3px double;padding:0 9.6px;\">Thin thick border (large)</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:3px double;padding:0 9.6px;\">Thin thick thin border (large)</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:1px solid;padding:0 9.6px;\">Wavy border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border:1px solid;padding:0 9.6px;\">Striped border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border-top:3px ridge;border-right:3px groove;border-bottom:3px groove;border-left:3px ridge;padding:0 9.6px;\">Emboss border</td></tr></table><p style=\"margin:0;\"><br></p><table style=\"border-spacing:0;font-size:inherit;box-sizing:border-box;\"><colgroup><col style=\"width:333.3px;\"></colgroup><tr><td style=\"vertical-align:top;border-top:3px groove;border-right:3px ridge;border-bottom:3px ridge;border-left:3px groove;padding:0 9.6px;\">Engrave border</td></tr></table><p style=\"margin:0;\"><br></p><p style=\"margin:0;\">End of border types.</p></div>");
    }

    private void TestConvert(string rtf, string html)
    {
      var actual = Rtf.ToHtml(rtf);
      ParseRender.AssertEqual(html, actual);
    }
  }
}
