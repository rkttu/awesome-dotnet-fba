#:sdk Microsoft.NET.Sdk
#:property OutputType=WinExe
#:property TargetFramework=net10.0-windows
#:property PublishAot=False
#:property UseWPF=False
#:property UseWindowsForms=True

// https://github.com/dotnet/winforms/issues/5071#issuecomment-908789632
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA); 

Application.OleRequired();
Application.EnableVisualStyles();
Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
Application.SetCompatibleTextRenderingDefault(false);

using var form = new Form()
{
    Text = $"Hello, World! - {Thread.CurrentThread.GetApartmentState()}",
    Width = 640,
    Height = 480,
    StartPosition = FormStartPosition.CenterScreen,
};

form.Controls.AddRange([
    new Label
    {
        Text = $"Hello, World! - {Thread.CurrentThread.GetApartmentState()}",
        Font = new Font(FontFamily.GenericSansSerif, 30f),
        TextAlign = ContentAlignment.MiddleCenter,
        Dock = DockStyle.Fill,
    }
]);

form.Controls.AddRange();

using var appContext = new ApplicationContext(form);
Application.Run(appContext);