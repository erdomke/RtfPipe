using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Converter.Html
{

	public class RtfHtmlSpecialCharCollection : Dictionary<RtfVisualSpecialCharKind, string>
	{

		public RtfHtmlSpecialCharCollection()
		{
		}

		public RtfHtmlSpecialCharCollection( string settings )
		{
			LoadSettings( settings );
		}

		public void LoadSettings( string settings )
		{
			Clear();
			if ( string.IsNullOrEmpty( settings ) )
			{
				return;
			}

			string[] settingItems = settings.Split( ',' );
			foreach ( string settingItem in settingItems )
			{
				string[] tokens = settingItem.Split( '=' );
				if ( tokens.Length != 2 )
				{
					continue;
				}

				RtfVisualSpecialCharKind charKind = (RtfVisualSpecialCharKind)Enum.Parse( typeof( RtfVisualSpecialCharKind ), tokens[ 0 ] );
				Add( charKind, tokens[ 1 ] );
			}
		}

		public string GetSettings()
		{
			if ( Count == 0 )
			{
				return string.Empty;
			}

			StringBuilder sb = new StringBuilder();
			foreach ( RtfVisualSpecialCharKind charKind in Keys )
			{
				if ( sb.Length > 0 )
				{
					sb.Append( ',' );
				}
				sb.Append( Enum.GetName( typeof( RtfVisualSpecialCharKind ), charKind ) );
				sb.Append( '=' );
				sb.Append( this[ charKind ] );
			}

			return sb.ToString();
		}

	}

}

