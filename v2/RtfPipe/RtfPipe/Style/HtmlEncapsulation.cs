using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  [Flags]
  public enum HtmlEncapsulation
  {
    /// <summary>Inside of a &lt;BODY&gt; HTML element and encapsulates a text fragment</summary>
    INBODY = 0x0000,
    /// <summary>Inside of a &lt;HEAD&gt; HTML element.</summary>
    INHEAD = 0x0001,
    /// <summary>Inside of an &lt;HTML&gt; HTML element.</summary>
    INHTML = 0x0002,
    /// <summary>Outside of an &lt;HTML&gt; HTML element.</summary>
    OUTHTML = 0x0003,
    ///<summary>This group encapsulates the &lt;HTML&gt; HTML element.</summary>
    HTML = 0x0010,
    ///<summary>This group encapsulates the &lt;HEAD&gt; HTML element.</summary>
    HEAD = 0x0020,
    ///<summary>This group encapsulates the &lt;BODY&gt; HTML element.</summary>
    BODY = 0x0030,
    ///<summary>This group encapsulates the &lt;P&gt; HTML element.</summary>
    P = 0x0040,
    ///<summary>This group encapsulates an HTML tag that starts a paragraph other than the &lt;P&gt; HTML element.</summary>
    STARTP = 0x0050,
    ///<summary>This group encapsulates an HTML tag that ends a paragraph other than the &lt;P&gt; HTML element.</summary>
    ENDP = 0x0060,
    ///<summary>This group encapsulates the &lt;BR&gt; HTML element.</summary>
    BR = 0x0070,
    ///<summary>This group encapsulates the &lt;PRE&gt; HTML element.</summary>
    PRE = 0x0080,
    ///<summary>This group encapsulates the &lt;FONT&gt; HTML element.</summary>
    FONT = 0x0090,
    ///<summary>This group encapsulates heading HTML tags such as &lt;H1&gt;, &lt;H2&gt;, and so on.</summary>
    HEADER = 0x00A0,
    ///<summary>This group encapsulates the &lt;TITLE&gt; HTML element.</summary>
    TITLE = 0x00B0,
    ///<summary>This group encapsulates the &lt;PLAIN&gt; HTML element.</summary>
    PLAIN = 0x00C0,
    ///<summary>Reserved, MUST be ignored.</summary>
    RESERVED1 = 0x00D0,
    ///<summary>Reserved, MUST be ignored.</summary>
    RESERVED2 = 0x00E0,
    ///<summary>This group encapsulates any other HTML tag.</summary>
    UNK = 0x00F0,
    ///<summary>The corresponding fragment of the original HTML SHOULD appear inside a paragraph HTML element.</summary>
    INPAR = 0x0004,
    ///<summary>This is a closing tag.</summary>
    CLOSE = 0x0008,
    ///<summary>This group encapsulates MIME Encapsulation of Aggregate HTML Documents (MHTML); that is, an HTML tag with a rewritable URL parameter. For more details about the MHTMLTAG destination group, see section 2.1.3.1.5.</summary>
    MHTML = 0x0100,

  }
}
