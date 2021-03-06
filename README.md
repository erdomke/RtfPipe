# RtfPipe

RtfPipe is a .NET library for parsing [Rich Text Format (RTF)](https://www.microsoft.com/en-us/download/details.aspx?id=10725) 
streams and converting them to HTML.  While initially adapted from the work started by 
[Jani Giannoudis](https://www.codeproject.com/Articles/27431/Writing-Your-Own-RTF-Converter), it has
been completely rewritten to support more features. When combined with the 
[BracketPipe](https://github.com/erdomke/BracketPipe/) library, this library
can also be used to convert RTF streams to various text format such as Markdown and Textile.

## Usage

Below is a simple example of converting an RTF string to an HTML string. 

> NOTE: When using this in .Net Core, be sure to include the NuGet package `System.Text.Encoding.CodePages`.
> Also call the line marked in the region before calling any functions in the library.

```csharp
#if NETCORE
  // Add a reference to the NuGet package System.Text.Encoding.CodePages for .Net core only
  Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
var html = Rtf.ToHtml(rtf);
```

## Installing via NuGet

[![NuGet version](https://badge.fury.io/nu/RtfPipe.svg)](https://www.nuget.org/packages/RtfPipe)

    Install-Package RtfPipe
    
## Building

Run `build.ps1` from the root of the project to build it.  The NuGet package will be output to the
`artifacts` directory.

## RTF Support

This library attempts to support the core RTF features documented in the 
[RTF Specification 1.9.1](https://www.microsoft.com/en-us/download/details.aspx?id=10725). These 
features include:

- Character formatting (bold, italics, color, ...)
- Tables (including nested tables)
- Lists
- Hyperlinks
- Pictures
- Heading levels
- HTML encapsulation (e.g. as performed by Outlook)

With that said, there are numerous cases for non-trivial documents where the library will not 
produce the "correct" visual representation when compared to other RTF readers (such as MS Word).

## .Net Targets

The NuGet package can be used with the following .Net targets

- .Net 3.5+
- .Net Standard 2.0+