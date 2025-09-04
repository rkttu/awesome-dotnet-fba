#!/usr/bin/env dotnet
#:sdk Microsoft.NET.Sdk
#:sdk Aspire.AppHost.Sdk@9.4.1
#:package Aspire.Hosting.AppHost@9.4.1
#:property UserSecretsId=0b081370-ed44-4dd5-b18a-20de2473cf09
#:property PublishAot=false

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    { "ASPNETCORE_URLS", "http://localhost:18888" },
    { "ASPIRE_DASHBOARD_OTLP_ENDPOINT_URL", "http://localhost:18889" },
    { "ASPIRE_DASHBOARD_OTLP_HTTP_ENDPOINT_URL", "http://localhost:18890" },
    { "ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true" },
});

// Running Aspire AppHost
// dotnet run -e DOTNET_ENVIRONMENT=Development -e MYSQL_ROOT_PASSWORD=aew!uwe5UTG7tzg0gqy -e MYSQL_DATABASE=wordpress -e MYSQL_USER=wordpress -e MYSQL_PASSWORD=vvuu42XUUGgN7thvodQZ .\wordpress.cs

// Export Manifest JSON
// dotnet run .\wordpress.cs --publisher manifest --output-path ./manifest.json

var mysqlRootPassword = builder.AddParameterFromConfiguration("MySqlRootPassword", "MYSQL_ROOT_PASSWORD", true);
var mysqlDatabase = builder.AddParameterFromConfiguration("MySqlDatabase", "MYSQL_DATABASE");
var mysqlUser = builder.AddParameterFromConfiguration("MySqlUser", "MYSQL_USER");
var mysqlPassword = builder.AddParameterFromConfiguration("MySqlPassword", "MYSQL_PASSWORD", true);

// MYSQL_USER, MYSQL_PASSWORD가 변경되면 볼륨을 삭제하고 다시 만들어야 함
var mysqlContainer = builder.AddContainer("mysql", "docker.io/mysql", "latest")
    .WithEndpoint(port: 3306, targetPort: 3306, name: "mysql-tcp")
    .WithEnvironment("MYSQL_ROOT_PASSWORD", mysqlRootPassword)
    .WithEnvironment("MYSQL_DATABASE", mysqlDatabase)
    .WithEnvironment("MYSQL_USER", mysqlUser)
    .WithEnvironment("MYSQL_PASSWORD", mysqlPassword)
    .WithVolume("mysql-data", "/var/lib/mysql") // 볼륨은 AppHost 종료 뒤에도 그대로 유지됨
    ;

var mysqlEndpoint = mysqlContainer.GetEndpoint("mysql-tcp");

_ = builder.AddContainer("phpmyadmin", "docker.io/phpmyadmin", "latest")
    .WithHttpEndpoint(port: 8080, targetPort: 80)
    .WithEnvironment("PMA_HOST", string.Join(':', mysqlContainer.Resource.Name, mysqlEndpoint?.TargetPort))
    .WithEnvironment("PMA_USER", "root")
    .WithEnvironment("PMA_PASSWORD", mysqlRootPassword)
    .WaitFor(mysqlContainer)
    ;

var wordpressContainer = builder.AddContainer("wordpress", "docker.io/wordpress", "latest")
    .WithHttpEndpoint(port: 8081, targetPort: 80, name: "wp-http")
    .WithEnvironment("WORDPRESS_DB_HOST", string.Join(':', mysqlContainer.Resource.Name, mysqlEndpoint?.TargetPort))
    .WithEnvironment("WORDPRESS_DB_NAME", mysqlDatabase)
    .WithEnvironment("WORDPRESS_DB_USER", mysqlUser)
    .WithEnvironment("WORDPRESS_DB_PASSWORD", mysqlPassword)
    .WithVolume("wp-data", "/var/www/html") // 볼륨은 AppHost 종료 뒤에도 그대로 유지됨
    .WaitFor(mysqlContainer)
    ;

var wordpressHttp = wordpressContainer.GetEndpoint("wp-http");

_ = builder.AddContainer("caddy", "docker.io/caddy", "latest")
    .WithHttpEndpoint(port: 80, targetPort: 80)
    .WithBindMount("Caddyfile", "/etc/caddy/Caddyfile", isReadOnly: true)
    .WaitFor(wordpressContainer);

wordpressContainer.WithEnvironment("WORDPRESS_CONFIG_EXTRA", """
if (isset($_SERVER['HTTP_X_FORWARDED_PROTO']) && $_SERVER['HTTP_X_FORWARDED_PROTO'] === 'https') {
    $_SERVER['HTTPS'] = 'on';
}
""");

File.WriteAllText("Caddyfile", $$"""
:80 {
    encode gzip
    reverse_proxy {{wordpressContainer.Resource.Name}}:{{wordpressHttp.TargetPort}}
    header {
        X-Forwarded-Proto {scheme}
    }
}
""");

builder.Build().Run();
