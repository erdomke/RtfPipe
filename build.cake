
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var versionSuffix = string.Format(".{0}.{1}"
	, (int)((DateTime.UtcNow - new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalDays)
	, (int)((DateTime.UtcNow - DateTime.UtcNow.Date).TotalSeconds / 2));
var version = "0.7" + versionSuffix;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
  .Does(() =>
{
  CleanDirectory(Directory("./RtfPipe/bin/" + configuration + "/"));
  CleanDirectory(Directory("./artifacts/"));
});

Task("Restore-NuGet-Packages")
  .IsDependentOn("Clean")
  .Does(() =>
{
	DeleteFiles("./**/project.json");
	DeleteFiles("./**/project.lock.json");
	DeleteFiles("./**/global.json");
  
  DotNetCoreRestore("./RtfPipe/RtfPipe.csproj");
});

Task("Build")
  .IsDependentOn("Restore-NuGet-Packages")
  .Does(() =>
{
  DotNetBuild("./RtfPipe/RtfPipe.csproj", (settings) =>
  {
     settings.Configuration = configuration;
     settings.WithProperty("VersionSuffix", versionSuffix);
  });
  
  MoveFiles("./RtfPipe/bin/" + configuration + "/RtfPipe.*.nupkg", "./artifacts/");
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
