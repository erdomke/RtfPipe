# RtfPipe

RtfPipe is a .NET library for parsing [Rich Text Format (RTF)](https://www.microsoft.com/en-us/download/details.aspx?id=10725) 
streams and converting them to HTML.  It is adapted from the work started by Jani Giannoudis which
is documented on [Code Project](https://www.codeproject.com/Articles/27431/Writing-Your-Own-RTF-Converter).
When combined with the [BracketPipe](https://github.com/erdomke/BracketPipe/) library, this library
can also be used to convert RTF streams to various text format such as Markdown and Textile.

## Usage

Below is a simple example of converting an RTF string to an HTML string. When using this in .Net Core, 
be sure to include the NuGet package `System.Text.Encoding.CodePages`.  Also call the line marked in the
region before calling any functions in the library.

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

## Portable

It is designed as a .Net Standard library targeting [.Net Standard 1.0](https://docs.microsoft.com/en-us/dotnet/articles/standard/library).  
That means it supports the following platforms:

* .NET Core 1.0+
* .NET Framework 4.5+
* Xamarin vNext+
* Universal Windows Platform 10.0+
* Windows 8.0+
* Windows Phone 8.1+
* Windows Phone Silverlight 8.0+

The NuGet package build also supports .Net 2.0 and .Net 4.0.

## License

Most of the code is covered under the [Code Project Open License](https://www.codeproject.com/info/cpol10.aspx).
The rest of the code should be considered freely open source.
