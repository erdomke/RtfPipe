using System;
using RtfPipe.Model;
using RtfPipe.Support;
using System.Collections.Generic;

namespace RtfPipe.Interpreter
{

	public sealed class RtfUserPropertyBuilder : RtfElementVisitorBase
	{

		public RtfUserPropertyBuilder( IList<IRtfDocumentProperty> collectedProperties ) :
			base( RtfElementVisitorOrder.NonRecursive )
		{
			// we iterate over our children ourselves -> hence non-recursive
			if ( collectedProperties == null )
			{
				throw new ArgumentNullException( "collectedProperties" );
			}
			this.collectedProperties = collectedProperties;
		}

		public IRtfDocumentProperty CreateProperty()
		{
			return new RtfDocumentProperty( propertyTypeCode, propertyName, staticValue, linkValue );
		}

		public void Reset()
		{
			propertyTypeCode = 0;
			propertyName = null;
			staticValue = null;
			linkValue = null;
		}

		protected override void DoVisitGroup( IRtfGroup group )
		{
			switch ( group.Destination )
			{
				case RtfSpec.TagUserProperties:
					VisitGroupChildren( group );
					break;
				case null:
					Reset();
					VisitGroupChildren( group );
					collectedProperties.Add( CreateProperty() );
					break;
				case RtfSpec.TagUserPropertyName:
					textBuilder.Reset();
					textBuilder.VisitGroup( group );
					propertyName = textBuilder.CombinedText;
					break;
				case RtfSpec.TagUserPropertyValue:
					textBuilder.Reset();
					textBuilder.VisitGroup( group );
					staticValue = textBuilder.CombinedText;
					break;
				case RtfSpec.TagUserPropertyLink:
					textBuilder.Reset();
					textBuilder.VisitGroup( group );
					linkValue = textBuilder.CombinedText;
					break;
			}
		}

		protected override void DoVisitTag( IRtfTag tag )
		{
			switch ( tag.Name )
			{
				case RtfSpec.TagUserPropertyType:
					propertyTypeCode = tag.ValueAsNumber;
					break;
			}
		}

		private readonly IList<IRtfDocumentProperty> collectedProperties;
		private readonly RtfTextBuilder textBuilder = new RtfTextBuilder();
		private int propertyTypeCode;
		private string propertyName;
		private string staticValue;
		private string linkValue;

	}

}

