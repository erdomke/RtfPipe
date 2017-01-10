// -- FILE ------------------------------------------------------------------
// name       : RtfHtmlCssStyleCollection.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.08
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Converter.Html
{

  // ------------------------------------------------------------------------
  public sealed class RtfHtmlCssStyleCollection : IRtfHtmlCssStyleCollection
  {
    private List<IRtfHtmlCssStyle> _innerList = new List<IRtfHtmlCssStyle>();

    public int Count
    {
      get { return _innerList.Count; }
    }

    // ----------------------------------------------------------------------
    public IRtfHtmlCssStyle this[ int index ]
    {
      get { return _innerList[ index ] as RtfHtmlCssStyle; }
    } // this[ int ]

    // ----------------------------------------------------------------------
    public bool Contains( string selectorName )
    {
      foreach ( var cssStyle in _innerList )
      {
        if ( cssStyle.SelectorName.Equals( selectorName ) )
        {
          return true;
        }
      }
      return false;
    } // Contains

    // ----------------------------------------------------------------------
    public void CopyTo( IRtfHtmlCssStyle[] array, int index )
    {
      _innerList.CopyTo( array, index );
    } // CopyTo

    // ----------------------------------------------------------------------
    public void Add( IRtfHtmlCssStyle item )
    {
      if ( item == null )
      {
        throw new ArgumentNullException( "item" );
      }
      _innerList.Add( item );
    } // Add

    // ----------------------------------------------------------------------
    public void Clear()
    {
      _innerList.Clear();
    } // Clear

    public IEnumerator<IRtfHtmlCssStyle> GetEnumerator()
    {
      return _innerList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  } // class RtfHtmlCssStyleCollection

} // namespace RtfPipe.Converter.Html
// -- EOF -------------------------------------------------------------------
