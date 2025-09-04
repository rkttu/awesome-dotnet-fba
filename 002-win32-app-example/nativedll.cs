#:property OutputType=Library
#:property RuntimeIdentifier=windows-x64

using System.Runtime.InteropServices;

internal static class NativeEntryPoints
{
    [UnmanagedCallersOnly(EntryPoint = "mymethod")]
    public static void MyMethod(nint ptrText)
    {
        if (ptrText == default)
            return;

        string? text = Marshal.PtrToStringUni(ptrText);
        Console.WriteLine($"{DateTime.Now} {text}");
    }
}
