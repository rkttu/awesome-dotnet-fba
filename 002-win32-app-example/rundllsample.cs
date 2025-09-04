#!/usr/bin/env dotnet
#:package ClosedXML@0.105.0
#:property OutputType=Library
#:property NativeLib=Shared
#:property RuntimeIdentifier=win-x64
#:property AllowUnsafeBlocks=true

// dotnet publish .\script.cs -o bin
// %windir%\system32\rundll32.exe bin\script.dll,EntryA "HelloWorld.xlsx"
// start HelloWorld.xlsx

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ClosedXML.Excel;

public static unsafe class Exports
{
    // ANSI version: rundll32 "<...>\RundllXlsx.dll",EntryA "C:\path\HelloWorld.xlsx"
    [UnmanagedCallersOnly(EntryPoint = "EntryA", CallConvs = new[] { typeof(CallConvStdcall) })]
    public static void EntryA(nint hwnd, nint hinst, sbyte* cmdLine, int nCmdShow)
        => Run(hwnd, PtrToManagedAnsi(cmdLine));

    private static void Run(nint hwnd, string? rawArgs)
    {
        try
        {
            var path = ParseFirstToken(rawArgs);
            if (string.IsNullOrWhiteSpace(path))
            {
                Show(hwnd,
                    "Usage:\nrundll32.exe \"<dll>\",EntryA \"C:\\out\\HelloWorld.xlsx\"",
                    "RundllXlsx - Missing Path");
                return;
            }

            path = Path.GetFullPath(path.Trim());
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

            CreateXlsx(path);

            Show(hwnd, $"XLSX written:\n{path}", "RundllXlsx - Success");
        }
        catch (Exception ex)
        {
            Show(hwnd, ex.ToString(), "RundllXlsx - Error");
        }
    }

    private static void CreateXlsx(string path)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Sample Sheet");
        ws.Cell("A1").Value = "Hello World!";
        ws.Cell("A2").FormulaA1 = "=MID(A1, 7, 5)";
        wb.SaveAs(path);
    }

    // ---- helpers ----
    private static string? PtrToManagedAnsi(sbyte* p)
        => p == null ? null : Marshal.PtrToStringAnsi((nint)p);
    private static string? PtrToManagedUni(char* p)
        => p == null ? null : Marshal.PtrToStringUni((nint)p);

    private static string? ParseFirstToken(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        s = s.Trim();
        if (s.StartsWith("\"") && s.EndsWith("\"") && s.Length >= 2)
            return s.Substring(1, s.Length - 2);
        var idx = s.IndexOf(' ');
        return idx < 0 ? s : s.Substring(0, idx);
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(nint hWnd, string text, string caption, uint type);

    private static void Show(nint hwnd, string text, string caption) => MessageBoxW(hwnd, text, caption, 0);
}
