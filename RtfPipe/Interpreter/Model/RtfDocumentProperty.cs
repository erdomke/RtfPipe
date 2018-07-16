using System;
using System.Text;
using RtfPipe.Sys;

namespace RtfPipe.Model
{

	public sealed class RtfDocumentProperty : IRtfDocumentProperty
	{

		public RtfDocumentProperty( int propertyKindCode, string name, string staticValue ) :
			this( propertyKindCode, name, staticValue, null )
		{
		}

		public RtfDocumentProperty( int propertyKindCode, string name, string staticValue, string linkValue )
		{
			if ( name == null )
			{
				throw new ArgumentNullException( "name" );
			}
			if ( staticValue == null )
			{
				throw new ArgumentNullException( "staticValue" );
			}
			this.propertyKindCode = propertyKindCode;
			switch ( propertyKindCode )
			{
				case RtfSpec.PropertyTypeInteger:
					propertyKind = RtfPropertyKind.IntegerNumber;
					break;
				case RtfSpec.PropertyTypeRealNumber:
					propertyKind = RtfPropertyKind.RealNumber;
					break;
				case RtfSpec.PropertyTypeDate:
					propertyKind = RtfPropertyKind.Date;
					break;
				case RtfSpec.PropertyTypeBoolean:
					propertyKind = RtfPropertyKind.Boolean;
					break;
				case RtfSpec.PropertyTypeText:
					propertyKind = RtfPropertyKind.Text;
					break;
				default:
					propertyKind = RtfPropertyKind.Unknown;
					break;
			}
			this.name = name;
			this.staticValue = staticValue;
			this.linkValue = linkValue;
		}

		public int PropertyKindCode
		{
			get { return propertyKindCode; }
		}

		public RtfPropertyKind PropertyKind
		{
			get { return propertyKind; }
		}

		public string Name
		{
			get { return name; }
		}

		public string StaticValue
		{
			get { return staticValue; }
		}

		public string LinkValue
		{
			get { return linkValue; }
		}

		public override bool Equals( object obj )
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

		private bool IsEqual( object obj )
		{
			RtfDocumentProperty compare = obj as RtfDocumentProperty; // guaranteed to be non-null
			return
				compare != null &&
				propertyKindCode == compare.propertyKindCode &&
				propertyKind == compare.propertyKind &&
				name.Equals( compare.name ) &&
				CompareTool.AreEqual( staticValue, compare.staticValue ) &&
				CompareTool.AreEqual( linkValue, compare.linkValue );
		}

		public override int GetHashCode()
		{
			return HashTool.AddHashCode( GetType().GetHashCode(), ComputeHashCode() );
		}

		private int ComputeHashCode()
		{
			int hash = propertyKindCode;
			hash = HashTool.AddHashCode( hash, propertyKind );
			hash = HashTool.AddHashCode( hash, name );
			hash = HashTool.AddHashCode( hash, staticValue );
			hash = HashTool.AddHashCode( hash, linkValue );
			return hash;
		}

		public override string ToString()
		{
			StringBuilder buf = new StringBuilder( name );
			if ( staticValue != null )
			{
				buf.Append( "=" );
				buf.Append( staticValue );
			}
			if ( linkValue != null )
			{
				buf.Append( "@" );
				buf.Append( linkValue );
			}
			return buf.ToString();
		}

		private readonly int propertyKindCode;
		private readonly RtfPropertyKind propertyKind;
		private readonly string name;
		private readonly string staticValue;
		private readonly string linkValue;

	}

}

