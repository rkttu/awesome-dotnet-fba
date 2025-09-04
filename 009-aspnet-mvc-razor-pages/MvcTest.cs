#!/usr/bin/env dotnet

// Run with dotnet run --no-cache MvcTest.cs

#:sdk Microsoft.NET.Sdk.Web
#:property PublishAot=False

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapGet("/ping", () => "Pong");

app.Run();

public class SamplePoco
{
    public DateTime CurrentDateTime { get; } = DateTime.Now;
}
