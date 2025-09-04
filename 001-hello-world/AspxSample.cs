#!/usr/bin/env dotnet
#:sdk Microsoft.NET.Sdk.Web

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Logging.AddConsole();

var app = builder.Build();
app.MapGet("/", () => "Hello, World!");
app.Run();