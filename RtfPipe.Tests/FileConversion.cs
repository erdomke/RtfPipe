using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtfPipe.Tests
{
  [TestClass]
  public class FileConversion
  {
    [TestMethod]
    public void RtfWithTables()
    {
      var rtf = System.IO.File.ReadAllText(@"C:\Users\eric.domke\Documents\Code\RtfPipe\RtfPipe.Tests\RtfTestDocument.rtf");
      var html = Rtf.ToHtml(rtf);
      Assert.AreEqual("", html);
    }

    [TestMethod]
    public void SimpleTable()
    {
      var rtf = @"{\rtf1\ansi\deff0
\trowd
\cellx1000
\cellx2000
\cellx3000
cell 1\intbl\cell
lots of text in cell two\intbl\cell
cell 3\intbl\cell
\row
}";
      var html = Rtf.ToHtml(rtf);
      Assert.AreEqual("", html);
    }
  }
}
