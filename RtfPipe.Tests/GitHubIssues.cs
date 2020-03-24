using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace RtfPipe.Tests
{
  [TestClass]
  public class GitHubIssues
  {
    [TestMethod]
    public void Issue10()
    {
      TestConvert(@"{\rtf1\ansi\ansicpg1252\deff0{\fonttbl{\f0\fnil\fcharset0 MS Sans Serif;}}
\viewkind4\uc1\pard\lang2057\f0\fs16\line Error can be found using this\par
}", @"<div style=""font-size:12pt;font-family:&quot;MS Sans Serif&quot;;""><br><p style=""font-size:8pt;margin:0;"">Error can be found using this</p></div>");
    }

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

    [TestMethod]
    public void Issue17()
    {
      const string rtf = @"{\rtf1\ansi\ansicpg1252\uc1\deff1{
\pict{\*\picprop\shplid1025{\sp{\sn shapeType}{\sv 75}}{\sp{\sn fFlipH}{\sv 0}}
{\sp{\sn fFlipV}{\sv 0}}{\sp{\sn fRotateText}{\sv 1}}{\sp{\sn pictureGray}{\sv 0}}{\sp{\sn pictureBiLevel}{\sv 0}}{\sp{\sn fFilled}{\sv 0}}{\sp{\sn fLine}{\sv 0}}{\sp{\sn wzName}{\sv Picture 0}}{\sp{\sn wzDescription}{\sv Linke
dIn.gif}}{\sp{\sn fHidden}{\sv 0}}{\sp{\sn fLayoutInCell}{\sv 1}}}\picscalex100\picscaley100\piccropl0\piccropr0\piccropt0\piccropb0\picw423\pich423\picwgoal240\pichgoal240\pngblip\bliptag-2141941385{\*\blipuid 805491778dc9ac8b2c298bd64da9d8ee}
89504e470d0a1a0a0000000d4948445200000010000000100803000000282d0f530000001974455874536f667477617265004d6963726f736f6674204f666669
63657fed357100000300504c54450000000f7fba0678b60b7ab70a7ab7137db80374b30676b40273b3157eb90d7bb70070b20072b3006eb00175b40f7cb80979
b60070b10778b6006db0006fb10074b40979b70777b5006caf0073b3006baf0676b50878b60274b30073b2006eb1006aae006cb00476b40d78b40b79b70d77b4
1e8abe1b85bd1281bb1a85bd1a84bc1d86bd1680ba1881bb1f84bc228abf258bbf3d97c72e90c22d90c23393c43192c43393c53293c52d90c32f91c33494c534
95c53595c53091c3248dc0268ec13f9bc82e91c3228bc02d91c33193c4248cc0268dc1228cc0258dc0328fc24099c9469cca489ecb489dcb4a9ecb4098c8489f
cb409bc84399c84f9fcb4499c850a0cd5ea9d25ca8d152a2cd52a3ce59a5cf5fa8d059a6ce5aa5ce78b4d77ab5d879b5d878b3d764aad27eb8da79b5d97bb6da
67abd27db9db60aad362abd37fbcdb69add38abfdd8cbfdd85bbdc82badc81badc84bbdc84bedc85bddc87bedc89bfdd84bddb86bedc82bcdb86bddc85bcdb8f
c3df8bc0dd8ac0dd8dc1de9ac9e39ecae39ac8e19ac9e2b1d7e9beddedb0d5e9a4cfe5a6d0e6bddbecbcdbecbadaecb9daeba3cee5c2dfeec7e1f0c7e2f0cde5
f1d4e8f3cfe5f2d8eaf4cee5f1cde4f1cce4f1cbe3f1d8ebf5c4e0eed7eaf4f9fdffeaf4f9e6f2f8e9f4f9f6fbfdf5fafdf2f8fceef6fbffffff010203010203
01020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301
02030102030102030102030102030102030102030102030102030102030102030102030102030102030102030102030102030102030102030102030102030102
03010203010203010203010203010203010203010203010203010203010203010203010203010203010203010203010203010203010203010203010203010203
01020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301020301
02030102030102030102030102037566e5d70000000174524e530040e6d86600000009704859730000004000000040006243635b0000000c636d50504a436d70
3037313200000003480073bc0000010549444154285363c8898b8f4f4880a2f8c45c86a4bce494d4fcfc0210c8cf2f4c63080d4befab0fcf8082cc080643affe
c5b323bd7d80c0d7c7c7cf9fc1c8386b4eb389a9a999b985a595b58d2d83be9dbd83a393b38bab5b40a09abb8701833a63d1dc168dd6796d1316cf6fd7d4d266
d061aa5e3c91b977f1e2050b172f8e62d1656065ab593c89bd63f1643dcf458b8b393819b8b84b164fe1e95cdcc0cb37757129373f83006fd9e269825d8b1b79
f9a62f2ee765661012ae583c4da47b7193b0e8f4c595c2620ce212b58b67f0ce5cdc2321396b719d940083b448505536777455b08864765588880c83ac9cbc82
84a4a282b0248f8482bc9c120383b20a12508d050054e2529f941a8ee10000000049454e44ae426082}
}";
      TestConvert(rtf, "<div style=\"font-size:12pt;\"><p style=\"margin:0;\"><img width=\"16\" height=\"16\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAMAAAAoLQ9TAAAAGXRFWHRTb2Z0d2FyZQBNaWNyb3NvZnQgT2ZmaWNlf+01cQAAAwBQTFRFAAAAD3+6Bni2C3q3Cnq3E324A3SzBna0AnOzFX65DXu3AHCyAHKzAG6wAXW0D3y4CXm2AHCxB3i2AG2wAG+xAHS0CXm3B3e1AGyvAHOzAGuvBna1CHi2AnSzAHOyAG6xAGquAGywBHa0DXi0C3m3DXe0Hoq+G4W9EoG7GoW9GoS8HYa9FoC6GIG7H4S8Ioq/JYu/PZfHLpDCLZDCM5PEMZLEM5PFMpPFLZDDL5HDNJTFNJXFNZXFMJHDJI3AJo7BP5vILpHDIovALZHDMZPEJIzAJo3BIozAJY3AMo/CQJnJRpzKSJ7LSJ3LSp7LQJjISJ/LQJvIQ5nIT5/LRJnIUKDNXqnSXKjRUqLNUqPOWaXPX6jQWabOWqXOeLTXerXYebXYeLPXZKrSfrjaebXZe7baZ6vSfbnbYKrTYqvTf7zbaa3Tir/djL/dhbvcgrrcgbrchLvchL7chb3ch77cib/dhL3bhr7cgrzbhr3chbzbj8Pfi8DdisDdjcHemsnjnsrjmsjhmsnisdfpvt3tsNXppM/lptDmvdvsvNvsutrsudrro87lwt/ux+Hwx+LwzeXx1Ojzz+Xy2Or0zuXxzeTxzOTxy+Px2Ov1xODu1+r0+f3/6vT55vL46fT59vv99fr98vj87vb7////AQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDdWbl1wAAAAF0Uk5TAEDm2GYAAAAJcEhZcwAAAEAAAABAAGJDY1sAAAAMY21QUEpDbXAwNzEyAAAAA0gAc7wAAAEFSURBVChTY8iJi49PSICi+MRchqS85JTU/PwCEMjPL0xjCA1L76sPz4CCzAgGQ6/+xbMjvX2AwNfHx8+fwcg4a06ziampmbmFpZW1jS2Dvp29g6OTs4urW0CgmruHAYM6Y9HcFo3WeW0TFs9v19TSZtBhql48kbl38eIFCxcvjmLRZWBlq1k8ib1j8WQ9z0WLizk4Gbi4SxZP4elc3MDLN3VxKTc/gwBv2eJpgl2LG3n5pi8u52VmEBKuWDxNpHtxk7Do9MWVwmIM4hK1i2fwzlzcIyE5a3GdlACDtEhQVTZ3dFWwiGR2VYiIDIOsnLyChKSigrAkj4SCvJwSA4OyChJQjQUAVOJSn5QajuEAAAAASUVORK5CYII=\"></p></div>");
      var parser = new Parser(rtf);
      var doc = parser.Parse();
    }

    [TestMethod]
    public void Issue18()
    {
      TestConvert(@"{\rtf1\ansi\ansicpg1252\fromhtml1 \fbidis \deff0{\fonttbl
{\f0\fswiss\fcharset0 Arial;}
{\f1\fmodern Courier New;}
{\f2\fnil\fcharset2 Symbol;}
{\f3\fmodern\fcharset0 Courier New;}
{\f4\fswiss\fcharset0 ""Segoe UI Emoji"";}}
{\colortbl\red0\green0\blue0;\red5\green99\blue193;}
\uc1\pard\plain\deftab360 \f0\fs24 
{\*\htmltag18 <html>}
{\*\htmltag50 <body lang=EN-US link=""#0563C1"" vlink=""#954F72"">}\htmlrtf \lang1033 \htmlrtf0 
{\*\htmltag96 <div class=WordSection1>}\htmlrtf {\htmlrtf0 
{\*\htmltag148 <span style='color:#7030A0'>}\htmlrtf {\htmlrtf0 Testing the smiley unicode 
{\*\htmltag156 </span>}\htmlrtf }\htmlrtf0 
{\*\htmltag148 <span style='font-family:""Segoe UI Emoji"",sans-serif;color:#7030A0'>}\htmlrtf {\f4 \htmlrtf0 \u-10179 ?\u-8694 ?
{\*\htmltag156 </span>}\htmlrtf }\htmlrtf0 
{\*\htmltag104 </div>}\htmlrtf }\htmlrtf0 
{\*\htmltag58 </body>}
{\*\htmltag27 </html>}}", @"<html><body lang=EN-US link=""#0563C1"" vlink=""#954F72""><div class=WordSection1><span style='color:#7030A0'>Testing the smiley unicode </span><span style='font-family:""Segoe UI Emoji"",sans-serif;color:#7030A0'>😊</span></div></body></html>");
    }

    [TestMethod]
    public void Issue20()
    {
      TestConvert(@"{\rtf1\ansi\ansicpg1252\lnbrkrule
{\fonttbl
{\f1\fswiss\fcharset0\fprq0 Arial;}
{\f2\fnil\fcharset134\fprq0 \'cb\'ce\'cc\'e5;}
{\f3\fnil\fcharset134\fprq0 \'ba\'da\'cc\'e5;}
{\f4\fswiss\fcharset161\fprq0 Arial;}
}
{\colortbl;
\red0\green0\blue0;
\red0\green0\blue255;
}
{\stylesheet
{\s1\qc\loch\af1\hich\af1\dbch\f2\fs44\ppscheme-3\lang1033\level1 heading 1;}
{\s2\li270\fi-270\loch\af1\hich\af1\dbch\f2\fs32\ppscheme-1\lang1033\level2 heading 2;}
{\s3\li585\fi-225\loch\af1\hich\af1\dbch\f2\fs28\ppscheme-1\lang1033\level3 heading 3;}
{\s4\li900\fi-180\loch\af1\hich\af1\dbch\f2\fs24\ppscheme-1\lang1033\level4 heading 4;}
{\s5\li1260\fi-180\loch\af1\hich\af1\dbch\f2\fs20\ppscheme-1\lang1033\level5 heading 5;}
{\s6\li1620\fi-180\loch\af1\hich\af1\dbch\f2\fs20\ppscheme-1\lang1033\level6 heading 6;}
}
\pard\plain\ltrpar\s1\qc\loch\af1\hich\af1\dbch\f2\fs44\ppscheme-3\lang1033 {\b\loch\af1\hich\af1\dbch\f2 \'a1\'b6}{\b\loch\af1\hich\af1\dbch\f2\lang2052\langfe2052 \'b6\'d4\'cd\'e2\'ba\'ba\'d3\'ef\'bd\'cc\'d1\'a7\'b8\'c5\'c2\'db}{\b\loch\af1\hich\af1\dbch\f2 \'a1\'b7}{\b\loch\af1\hich\af1\dbch\f2 \par 
}
}", @"<div style=""font-size:12pt;""><p style=""text-align:center;font-size:22pt;font-family:宋体;margin:0;""><strong>《对外汉语教学概论》</strong></p></div>");
    }

    [TestMethod]
    public void Issue23()
    {
      TestConvert("RtfPipe.Tests.Files.Issue23");
    }

    [TestMethod]
    public void Issue25()
    {
      TestConvert(@"{\rtf1\ansi\ansicpg1252\deff0\deflang3081\deflangfe3081\paperw11906\paperh16838\margl851\margr851\margt851\margb851{\fonttbl{\f0\fnil\fcharset0 Courier New;}}{\colortbl ;\red0\green0\blue0;\red0\green0\blue255;\red0\green153\blue0;\red0\green255\blue255;\red255\green0\blue0;\red255\green0\blue255;\red102\green51\blue0;\red192\green192\blue192;\red96\green96\blue96;\red51\green153\blue255;\red51\green255\blue51;\red153\green255\blue255;\red255\green255\blue0;\red255\green255\blue255;}\pard\plain \ql\li0\ri0\nowidctlpar\sl276\slmult1\f0\fs20\lang3081\par \par \par \par 28th February, 2007.\par \par Dr Donald Duck\par Disney General Practice\par 1011 Bourbon Street\par Random Town VIC 3377\par \par Dear Pete\par \par 	RE: Mr John Smith (DOB: 25/02/39)\par 	Unit 15/246 Spencer Street Random Town VIC 3377\par \par John is at sixes and sevens.   He is very anxious and has a whole load of psychosomatic complaints.   None of these are related to his actual prostate cancer.\par \par His bone scan reveals some arthritis in his shoulders and an old rib fracture on the right.   There is no evidence of metastatic disease.   This is consistent with expectation from his pathology.\par \par His options are watchful waiting or external beam radiotherapy.   He is excluded from Brachy therapy because he has elements of Gleason 4 carcinoma.   Surgery would not be a good option in him due to a significant CVA seven to eight years ago and this would put his risks of repeat up considerably.\par \par He is due for review by you shortly and I would appreciate it if you could treat him in a general sense.   I will catch-up with him for further discussion in two to three weeks time.\par \par Kind regards\par \par David Dodge\par }"
, @"<div style=""font-size:12pt;font-family:&quot;Courier New&quot;;""><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">28th February, 2007.</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">Dr Donald Duck</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">Disney General Practice</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">1011 Bourbon Street</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">Random Town VIC 3377</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">Dear Pete</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;text-indent:48px;margin:0;"">RE: Mr John Smith (DOB: 25/02/39)</p><p style=""text-align:left;line-height:1.4;font-size:10pt;text-indent:48px;margin:0;"">Unit 15/246 Spencer Street Random Town VIC 3377</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">John is at sixes and sevens. &nbsp;&nbsp;He is very anxious and has a whole load of psychosomatic complaints. &nbsp;&nbsp;None of these are related to his actual prostate cancer.</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">His bone scan reveals some arthritis in his shoulders and an old rib fracture on the right. &nbsp;&nbsp;There is no evidence of metastatic disease. &nbsp;&nbsp;This is consistent with expectation from his pathology.</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">His options are watchful waiting or external beam radiotherapy. &nbsp;&nbsp;He is excluded from Brachy therapy because he has elements of Gleason 4 carcinoma. &nbsp;&nbsp;Surgery would not be a good option in him due to a significant CVA seven to eight years ago and this would put his risks of repeat up considerably.</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">He is due for review by you shortly and I would appreciate it if you could treat him in a general sense. &nbsp;&nbsp;I will catch-up with him for further discussion in two to three weeks time.</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">Kind regards</p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;""><br></p><p style=""text-align:left;line-height:1.4;font-size:10pt;margin:0;"">David Dodge</p></div>");
    }

    [TestMethod]
    public void Issue26()
    {
      TestConvert(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033\fs20{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}
{\f99\froman\fcharset0\fprq2{\*\panose 02020603050405020304}Arial;}{\f100\fnil\fcharset2 Symbol;}{\f101\fnil\fcharset2 Wingdings;}{\f102\fcharset204{\*\fname Courier New;}Courier New CYR;}{\f103\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue0;\red51\green102\blue255;}
\paperw12240\paperh15840\margl1417\margr1134\margt1134\margb1134
\pard\sb100\sa100\sbauto1\saauto1\fs20\lang1033
ist Ersatzabruf für 4200028332 warAN 68595 bez 12.261,20- Re 111607-7105658060-ok !!\pard}}", @"<div style=""font-size:12pt;font-family:Arial, sans-serif;""><p style=""font-size:10pt;margin:6.7px 0 6.7px 0;"">ist Ersatzabruf für 4200028332 warAN 68595 bez 12.261,20- Re 111607-7105658060-ok !!</p></div>");
    }

    [TestMethod]
    public void Issue29()
    {
      TestConvert(@"{\rtf1\ansi\deff0 {\fonttbl {\f0 Times New Roman;}}
\f0\fs60 Copies:\par
Dr G. Smith\par \par
PELVIS \ LEFT HIP 01/04/2019 Reference: 1234556 NHI: ABC1234\par
AC Reference: 12312443423\par \par \par
}", @"<div style=""font-size:12pt;font-family:&quot;Times New Roman&quot;;""><p style=""font-size:30pt;margin:0;"">Copies:</p><p style=""font-size:30pt;margin:0;"">Dr G. Smith</p><p style=""font-size:30pt;margin:0;""><br></p><p style=""font-size:30pt;margin:0;"">PELVIS LEFT HIP 01/04/2019 Reference: 1234556 NHI: ABC1234</p><p style=""font-size:30pt;margin:0;"">AC Reference: 12312443423</p><p style=""font-size:30pt;margin:0;""><br></p><p style=""font-size:30pt;margin:0;""><br></p></div>");
    }

    [TestMethod]
    public void Issue30()
    {
      TestConvert(@"{\rtf1\ansi\deff0 {\fonttbl {\f0 Times New Roman;}}
\f0\fs60 Hello.br\.br, World!
}", @"<div style=""font-size:12pt;font-family:&quot;Times New Roman&quot;;""><p style=""font-size:30pt;margin:0;"">Hello.brbr, World!</p></div>");
    }

    [TestMethod]
    public void Issue32_Corrected()
    {
      TestConvert(@"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang2057{\fonttbl{\f0\fnil\fcharset0 Microsoft Sans Serif;}}
{\colortbl ;\red0\green0\blue255;\red0\green0\blue0;}
{\*\generator Riched20 10.0.18362}\viewkind4\uc1
\pard {\f0\fs17{\field{\*\fldinst{HYPERLINK http://www.xxxx.co.uk }}{\fldrslt{http://www.xxxx.co.uk\ul0\cf0}}}}\cf2\f0\fs17\par
\par
\par}", @"<div style=""font-size:12pt;font-family:&quot;Microsoft Sans Serif&quot;;""><p style=""font-size:8.5pt;margin:0;""><a style=""text-decoration:none;"" href=""http://www.xxxx.co.uk"">http://www.xxxx.co.uk</a></p><p style=""font-size:8.5pt;margin:0;""><br></p><p style=""font-size:8.5pt;margin:0;""><br></p></div>");
    }

    [TestMethod]
    public void Issue33()
    {
      TestConvert("RtfPipe.Tests.Files.Issue33");
    }

    [TestMethod]
    public void Issue34()
    {
      TestConvert("RtfPipe.Tests.Files.Issue34");
    }

    [TestMethod]
    public void Issue35()
    {
      TestConvert("RtfPipe.Tests.Files.Issue35");
    }

    [TestMethod]
    public void Issue37()
    {
      TestConvert("RtfPipe.Tests.Files.Issue37");
    }

    //[TestMethod]
    //public void Issue38()
    //{
    //  TestConvert(@"{\rtf1\ansi\ansicpg1252\uc1\htmautsp\deff2{\fonttbl{\f0\fcharset0 Times New Roman;}{\f2\fcharset0 Segoe UI;}{\f3\fcharset0 Arial;}}{\colortbl\red0\green0\blue0;\red255\green255\blue255;}\loch\hich\dbch\pard\plain\ltrpar\itap0{\lang1033\fs20\f3\cf0 \cf0\ql{\f3 {\ltrch Left}\li0\ri0\sa160\sb0\fi0\ql\par} {\f3 {\ltrch Center}\li0\ri0\sa160\sb0\fi0\qc\par} {\f3 {\ltrch Right}\li0\ri0\sa160\sb0\fi0\qr\par} } }"
    //    , "");
    //}

    [TestMethod]
    public void Issue39()
    {
      TestConvert("RtfPipe.Tests.Files.Issue39");
    }

    private void TestConvert(RtfSource rtf, string html)
    {
      var actual = Rtf.ToHtml(rtf);
      Assert.AreEqual(html, actual);
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
  }
}
