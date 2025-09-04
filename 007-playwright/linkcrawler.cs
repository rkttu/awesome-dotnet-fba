#!/usr/bin/env dotnet
#:sdk Microsoft.NET.Sdk.Web
#:property PublishAot=False
#:package Microsoft.Playwright@1.54.0

// Synopsis
// dotnet run crawler.cs (TargetUrl will be https://forum.dotnetdev.kr/)
// dotnet run crawler.cs --targeturl=https://www.naver.com/

using Microsoft.Playwright;
using PlaywrightProgram = Microsoft.Playwright.Program;

// Install Playwright dependencies
Console.WriteLine("Installing playwright dependencies...");
PlaywrightProgram.Main(["install"]);

using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true, });
var page = await browser.NewPageAsync();

var configLoader = new ConfigurationBuilder();
configLoader.AddEnvironmentVariables().AddCommandLine(args);
var config = configLoader.Build();

if (!Uri.TryCreate(config["TargetUrl"], UriKind.Absolute, out var targetUrl) || targetUrl == null)
    targetUrl = new Uri("https://forum.dotnetdev.kr/");

Console.WriteLine($"Navigating {targetUrl} site...");

await page.GotoAsync(targetUrl.AbsoluteUri);
await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

var allLinks = await page.QuerySelectorAllAsync("a");
var foundLinks = new HashSet<string>();
foreach (var eachLink in allLinks)
{
    var hrefValue = (await eachLink.GetAttributeAsync("href") ?? string.Empty).Trim();
    if (Uri.TryCreate(hrefValue, UriKind.RelativeOrAbsolute, out var createdUri) && createdUri != null)
    {
        var normalizedUri = (createdUri.IsAbsoluteUri ? createdUri : new Uri(targetUrl, createdUri)).AbsoluteUri;
        if (!foundLinks.Contains(normalizedUri, StringComparer.Ordinal))
            foundLinks.Add(normalizedUri);
    }
}

foreach (var eachLink in foundLinks)
    Console.WriteLine(eachLink);
