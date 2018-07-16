﻿using System;
using RtfPipe.Model;
using RtfPipe.Support;

namespace RtfPipe.Interpreter
{

	public sealed class RtfDocumentInfoBuilder : RtfElementVisitorBase
	{

		public RtfDocumentInfoBuilder( RtfDocumentInfo info ) :
			base( RtfElementVisitorOrder.NonRecursive )
		{
			// we iterate over our children ourselves -> hence non-recursive
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}
			this.info = info;
		}

		public void Reset()
		{
			info.Reset();
		}

		protected override void DoVisitGroup( IRtfGroup group )
		{
			switch ( group.Destination )
			{
				case RtfSpec.TagInfo:
					VisitGroupChildren( group );
					break;
				case RtfSpec.TagInfoTitle:
					info.Title = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoSubject:
					info.Subject = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoAuthor:
					info.Author = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoManager:
					info.Manager = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoCompany:
					info.Company = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoOperator:
					info.Operator = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoCategory:
					info.Category = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoKeywords:
					info.Keywords = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoComment:
					info.Comment = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoDocumentComment:
					info.DocumentComment = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoHyperLinkBase:
					info.HyperLinkbase = ExtractGroupText( group );
					break;
				case RtfSpec.TagInfoCreationTime:
					info.CreationTime = ExtractTimestamp( group );
					break;
				case RtfSpec.TagInfoRevisionTime:
					info.RevisionTime = ExtractTimestamp( group );
					break;
				case RtfSpec.TagInfoPrintTime:
					info.PrintTime = ExtractTimestamp( group );
					break;
				case RtfSpec.TagInfoBackupTime:
					info.BackupTime = ExtractTimestamp( group );
					break;
			}
		}

		protected override void DoVisitTag( IRtfTag tag )
		{
			switch ( tag.Name )
			{
				case RtfSpec.TagInfoVersion:
					info.Version = tag.ValueAsNumber;
					break;
				case RtfSpec.TagInfoRevision:
					info.Revision = tag.ValueAsNumber;
					break;
				case RtfSpec.TagInfoNumberOfPages:
					info.NumberOfPages = tag.ValueAsNumber;
					break;
				case RtfSpec.TagInfoNumberOfWords:
					info.NumberOfWords = tag.ValueAsNumber;
					break;
				case RtfSpec.TagInfoNumberOfChars:
					info.NumberOfCharacters = tag.ValueAsNumber;
					break;
				case RtfSpec.TagInfoId:
					info.Id = tag.ValueAsNumber;
					break;
				case RtfSpec.TagInfoEditingTimeMinutes:
					info.EditingTimeInMinutes = tag.ValueAsNumber;
					break;
			}
		}

		private string ExtractGroupText( IRtfGroup group )
		{
			textBuilder.Reset();
			textBuilder.VisitGroup( group );
			return textBuilder.CombinedText;
		}

		private DateTime ExtractTimestamp( IRtfGroup group )
		{
			timestampBuilder.Reset();
			timestampBuilder.VisitGroup( group );
			return timestampBuilder.CreateTimestamp();
		}

		private readonly RtfDocumentInfo info;
		private readonly RtfTextBuilder textBuilder = new RtfTextBuilder();
		private readonly RtfTimestampBuilder timestampBuilder = new RtfTimestampBuilder();

	}

}

