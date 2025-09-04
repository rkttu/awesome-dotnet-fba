#!/usr/bin/env dotnet
#:sdk Microsoft.NET.Sdk.Web
#:package ModelContextProtocol@0.3.0-preview.4

// 노트: dotnet run .cs 명령으로는 이 MCP 서버를 동시에 여러 클라이언트에서 사용하지 못할 수 있습니다.

using System.ComponentModel;
using ModelContextProtocol.Server;

var builder = Host.CreateEmptyApplicationBuilder(default);
builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHttpClient();
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithTools([
        McpServerTool.Create(IpAddressTool),
    ]);

var app = builder.Build();
app.Run();

[Description("Get the public IP address of this machine.")]
async Task<string> IpAddressTool(
    IServiceProvider services,
    [Description("Get IPv6 address instead of IPv4 address")] bool ipv6)
{
    try
    {
        var client = services.GetRequiredService<HttpClient>();
        return await client.GetStringAsync(
            ipv6 ? "https://api6.ipify.org" : "https://api.ipify.org"
            ).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
        return $"Error occurred: {ex.Message}";
    }
}
