#:property PublishAot=False
#:property PackAsTool=True
#:property ToolCommaneName=sampletool

// Steps to generate the nuget package:
// - dotnet project convert -o sampletool sampletool.cs
// - dotnet pack ./sampletool
// - copy ./sampletool/bin/Release/sampletool*.nupkg .
// - rm -rf ./sampletool/

Console.WriteLine("Sample Tool");
