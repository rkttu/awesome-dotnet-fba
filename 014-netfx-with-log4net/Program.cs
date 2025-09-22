#:property TargetFramework=net48

#:property PublishAot=False
#:property Nullable=Disable
#:property ImplicitUsings=Disable
#:property LangVersion=7.3

#:package log4net@3.2.0

// Run with: dotnet run --no-cache Program.cs

using System;
using System.Configuration;
using System.Xml.Linq;
using log4net;
using log4net.Config;

internal static class Program
{
    private static ILog log = LogManager.GetLogger(typeof(Program));

    [STAThread]
    private static void Main(string[] args)
    {
        XmlConfigurator.Configure();
        log.Debug("Main() started");

        try
        {
            var targetUrl = ConfigurationManager.AppSettings["TargetUrl"];
            log.InfoFormat("Target URL: {0}", targetUrl);
            var doc = XDocument.Load(targetUrl);
            log.Info(doc.ToString());
        }
        catch (Exception ex)
        {
            log.Error("Cannot load XML document.", ex);
        }
    }
}
