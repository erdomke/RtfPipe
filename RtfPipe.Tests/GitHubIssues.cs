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

    [TestMethod]
    public void Issue17()
    {
      TestConvert(@"{\rtf1\ansi\ansicpg1252\uc1\deff1{
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
}", "<div style=\"font-size:12pt;\"><p style=\"margin:0;\"><img width=\"16\" height=\"16\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAMAAAAoLQ9TAAAAGXRFWHRTb2Z0d2FyZQBNaWNyb3NvZnQgT2ZmaWNlf+01cQAAAwBQTFRFAAAAD3+6Bni2C3q3Cnq3E324A3SzBna0AnOzFX65DXu3AHCyAHKzAG6wAXW0D3y4CXm2AHCxB3i2AG2wAG+xAHS0CXm3B3e1AGyvAHOzAGuvBna1CHi2AnSzAHOyAG6xAGquAGywBHa0DXi0C3m3DXe0Hoq+G4W9EoG7GoW9GoS8HYa9FoC6GIG7H4S8Ioq/JYu/PZfHLpDCLZDCM5PEMZLEM5PFMpPFLZDDL5HDNJTFNJXFNZXFMJHDJI3AJo7BP5vILpHDIovALZHDMZPEJIzAJo3BIozAJY3AMo/CQJnJRpzKSJ7LSJ3LSp7LQJjISJ/LQJvIQ5nIT5/LRJnIUKDNXqnSXKjRUqLNUqPOWaXPX6jQWabOWqXOeLTXerXYebXYeLPXZKrSfrjaebXZe7baZ6vSfbnbYKrTYqvTf7zbaa3Tir/djL/dhbvcgrrcgbrchLvchL7chb3ch77cib/dhL3bhr7cgrzbhr3chbzbj8Pfi8DdisDdjcHemsnjnsrjmsjhmsnisdfpvt3tsNXppM/lptDmvdvsvNvsutrsudrro87lwt/ux+Hwx+LwzeXx1Ojzz+Xy2Or0zuXxzeTxzOTxy+Px2Ov1xODu1+r0+f3/6vT55vL46fT59vv99fr98vj87vb7////AQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDAQIDdWbl1wAAAAF0Uk5TAEDm2GYAAAAJcEhZcwAAAEAAAABAAGJDY1sAAAAMY21QUEpDbXAwNzEyAAAAA0gAc7wAAAEFSURBVChTY8iJi49PSICi+MRchqS85JTU/PwCEMjPL0xjCA1L76sPz4CCzAgGQ6/+xbMjvX2AwNfHx8+fwcg4a06ziampmbmFpZW1jS2Dvp29g6OTs4urW0CgmruHAYM6Y9HcFo3WeW0TFs9v19TSZtBhql48kbl38eIFCxcvjmLRZWBlq1k8ib1j8WQ9z0WLizk4Gbi4SxZP4elc3MDLN3VxKTc/gwBv2eJpgl2LG3n5pi8u52VmEBKuWDxNpHtxk7Do9MWVwmIM4hK1i2fwzlzcIyE5a3GdlACDtEhQVTZ3dFWwiGR2VYiIDIOsnLyChKSigrAkj4SCvJwSA4OyChJQjQUAVOJSn5QajuEAAAAASUVORK5CYII=\"></p></div>");
    }

    private void TestConvert(string rtf, string html)
    {
      var actual = Rtf.ToHtml(rtf);
      Assert.AreEqual(html, actual);
    }
  }
}
