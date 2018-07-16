using RtfPipe.Sys;

namespace RtfPipe.Converter.Html
{

	public class RtfHtmlStyle : IRtfHtmlStyle
	{

		public static RtfHtmlStyle Empty = new RtfHtmlStyle();

		public string ForegroundColor
		{
			get { return foregroundColor; }
			set { foregroundColor = value; }
		}

		public string BackgroundColor
		{
			get { return backgroundColor; }
			set { backgroundColor = value; }
		}

		public string FontFamily
		{
			get { return fontFamily; }
			set { fontFamily = value; }
		}

		public string FontSize
		{
			get { return fontSize; }
			set { fontSize = value; }
		}

		public bool IsEmpty
		{
			get { return Equals( Empty ); }
		}

		public sealed override bool Equals( object obj )
		{
			if ( obj == this )
			{
				return true;
			}
			
			if ( obj == null || GetType() != obj.GetType() )
			{
				return false;
			}

			return IsEqual( obj );
		}

		public sealed override int GetHashCode()
		{
			return HashTool.AddHashCode( GetType().GetHashCode(), ComputeHashCode() );
		}

		private bool IsEqual( object obj )
		{
			RtfHtmlStyle compare = obj as RtfHtmlStyle; // guaranteed to be non-null
			return
				compare != null &&
				string.Equals( foregroundColor, compare.foregroundColor ) &&
				string.Equals( backgroundColor, compare.backgroundColor ) &&
				string.Equals( fontFamily, compare.fontFamily ) &&
				string.Equals( fontSize, compare.fontSize );
		}

		private int ComputeHashCode()
		{
			int hash = foregroundColor.GetHashCode();
			hash = HashTool.AddHashCode( hash, backgroundColor );
			hash = HashTool.AddHashCode( hash, fontFamily );
			hash = HashTool.AddHashCode( hash, fontSize );
			return hash;
		}

		private string foregroundColor;
		private string backgroundColor;
		private string fontFamily;
		private string fontSize;

	}

}

