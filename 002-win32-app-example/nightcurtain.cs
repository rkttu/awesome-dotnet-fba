#!/usr/bin/env dotnet
#:sdk Microsoft.NET.Sdk
#:property OutputType=WinExe
#:property TargetFramework=net10.0-windows

// This program can be scheduled using Windows Task Scheduler.
// Below are some example `schtasks` CLI commands to register tasks for `nightcurtain.exe`.
//
// 1. Enable color filter every night at 10:00 PM
// schtasks /Create /TN "NightCurtain_Enable" /TR "C:\Path\To\nightcurtain.exe enable" /SC DAILY /ST 22:00 /F
//
// 2. Disable color filter every morning at 6:00 AM
// schtasks /Create /TN "NightCurtain_Disable" /TR "C:\Path\To\nightcurtain.exe disable" /SC DAILY /ST 06:00 /F
//
// 3. Run automatically on system startup (time-based mode)
// schtasks /Create /TN "NightCurtain_AtStartup" /TR "C:\Path\To\nightcurtain.exe" /SC ONSTART /F
//
// 4. Run automatically when the user logs in (time-based mode)
// schtasks /Create /TN "NightCurtain_AtLogon" /TR "C:\Path\To\nightcurtain.exe" /SC ONLOGON /F
//
// Notes:
// - Replace `C:\Path\To\nightcurtain.exe` with the actual path.
// - When no arguments are passed, the program uses time-based logic: it enables the filter between 10 PM and 6 AM.

using Microsoft.Win32;
using System.Diagnostics;

#if LINQPAD
var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
#endif

var currentDateTime = DateTime.Now;

var atBrokerPath = Path.Combine(
	Environment.GetFolderPath(Environment.SpecialFolder.System),
	"atbroker.exe");

if (!File.Exists(atBrokerPath))
{
	Console.Error.WriteLine("This program requires the Assistive Technology Manager utility in Windows.");
	Environment.Exit(1);
	return;
}

using var colorFilteringSubKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\ColorFiltering");
if (colorFilteringSubKey != null)
{
	var active = 0;

	var firstArg = args.FirstOrDefault()?.Trim()?.ToLower() ?? string.Empty;
	if (firstArg.Equals("enable", StringComparison.OrdinalIgnoreCase))
		active = 1;
	else if (firstArg.Equals("disable", StringComparison.OrdinalIgnoreCase))
		active = 0;
	else
		active = (22 <= currentDateTime.Hour || currentDateTime.Hour < 6) ? 1 : 0;

	colorFilteringSubKey.SetValue("Active", active, RegistryValueKind.DWord);
	colorFilteringSubKey.SetValue("FilterType", 0, RegistryValueKind.DWord);
}

// https://learn.microsoft.com/en-us/answers/questions/862266/any-net-api-available-to-toggle-the-color-filter-w
var startInfo = new ProcessStartInfo(atBrokerPath)
{
	CreateNoWindow = true,
	UseShellExecute = false,
	Verb = "runas",
};
startInfo.ArgumentList.Add("/colorfiltershortcut");
startInfo.ArgumentList.Add("/resettransferkeys");

using var process = Process.Start(startInfo);
if (process != null)
	process.WaitForExit();
