using System;

namespace RtfPipe.Model
{
  public sealed class RtfDocumentInfo : IRtfDocumentInfo
  {
    public int? Id { get; set; }

    public int? Version { get; set; }

    public int? Revision { get; set; }

    public string Title { get; set; }

    public string Subject { get; set; }

    public string Author { get; set; }

    public string Manager { get; set; }

    public string Company { get; set; }

    public string Operator { get; set; }

    public string Category { get; set; }

    public string Keywords { get; set; }

    public string Comment { get; set; }

    public string DocumentComment { get; set; }

    public string HyperLinkbase { get; set; }

    public DateTime? CreationTime { get; set; }

    public DateTime? RevisionTime { get; set; }

    public DateTime? PrintTime { get; set; }

    public DateTime? BackupTime { get; set; }

    public int? NumberOfPages { get; set; }

    public int? NumberOfWords { get; set; }

    public int? NumberOfCharacters { get; set; }

    public int? EditingTimeInMinutes { get; set; }

    public void Reset()
    {
      Id = null;
      Version = null;
      Revision = null;
      Title = null;
      Subject = null;
      Author = null;
      Manager = null;
      Company = null;
      Operator = null;
      Category = null;
      Keywords = null;
      Comment = null;
      DocumentComment = null;
      HyperLinkbase = null;
      CreationTime = null;
      RevisionTime = null;
      PrintTime = null;
      BackupTime = null;
      NumberOfPages = null;
      NumberOfWords = null;
      NumberOfCharacters = null;
      EditingTimeInMinutes = null;
    }
  }
}

