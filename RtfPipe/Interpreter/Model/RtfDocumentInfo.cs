using System;

namespace RtfPipe.Model
{

	public sealed class RtfDocumentInfo : IRtfDocumentInfo
	{

		public int? Id
		{
			get { return id; }
			set { id = value; }
		}

		public int? Version
		{
			get { return version; }
			set { version = value; }
		}

		public int? Revision
		{
			get { return revision; }
			set { revision = value; }
		}

		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		public string Subject
		{
			get { return subject; }
			set { subject = value; }
		}

		public string Author
		{
			get { return author; }
			set { author = value; }
		}

		public string Manager
		{
			get { return manager; }
			set { manager = value; }
		}

		public string Company
		{
			get { return company; }
			set { company = value; }
		}

		public string Operator
		{
			get { return operatorName; }
			set { operatorName = value; }
		}

		public string Category
		{
			get { return category; }
			set { category = value; }
		}

		public string Keywords
		{
			get { return keywords; }
			set { keywords = value; }
		}

		public string Comment
		{
			get { return comment; }
			set { comment = value; }
		}

		public string DocumentComment
		{
			get { return documentComment; }
			set { documentComment = value; }
		}

		public string HyperLinkbase
		{
			get { return hyperLinkbase; }
			set { hyperLinkbase = value; }
		}

		public DateTime? CreationTime
		{
			get { return creationTime; }
			set { creationTime = value; }
		}

		public DateTime? RevisionTime
		{
			get { return revisionTime; }
			set { revisionTime = value; }
		}

		public DateTime? PrintTime
		{
			get { return printTime; }
			set { printTime = value; }
		}

		public DateTime? BackupTime
		{
			get { return backupTime; }
			set { backupTime = value; }
		}

		public int? NumberOfPages
		{
			get { return numberOfPages; }
			set { numberOfPages = value; }
		}

		public int? NumberOfWords
		{
			get { return numberOfWords; }
			set { numberOfWords = value; }
		}

		public int? NumberOfCharacters
		{
			get { return numberOfCharacters; }
			set { numberOfCharacters = value; }
		}

		public int? EditingTimeInMinutes
		{
			get { return editingTimeInMinutes; }
			set { editingTimeInMinutes = value; }
		}

		public void Reset()
		{
			id = null;
			version = null;
			revision = null;
			title = null;
			subject = null;
			author = null;
			manager = null;
			company = null;
			operatorName = null;
			category = null;
			keywords = null;
			comment = null;
			documentComment = null;
			hyperLinkbase = null;
			creationTime = null;
			revisionTime = null;
			printTime = null;
			backupTime = null;
			numberOfPages = null;
			numberOfWords = null;
			numberOfCharacters = null;
			editingTimeInMinutes = null;
		}

		public override string ToString()
		{
			return "RTFDocInfo";
		}

		private int? id;
		private int? version;
		private int? revision;
		private string title;
		private string subject;
		private string author;
		private string manager;
		private string company;
		private string operatorName;
		private string category;
		private string keywords;
		private string comment;
		private string documentComment;
		private string hyperLinkbase;
		private DateTime? creationTime;
		private DateTime? revisionTime;
		private DateTime? printTime;
		private DateTime? backupTime;
		private int? numberOfPages;
		private int? numberOfWords;
		private int? numberOfCharacters;
		private int? editingTimeInMinutes;

	}

}

