#:package pythonnet@3.0.5
#:property PublishAot=False

using Python.Runtime;

var versionPostfix = "312";
Runtime.PythonDLL = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    @$"Programs\Python\Python{versionPostfix}\python{versionPostfix}.dll");
RuntimeData.FormatterType = typeof(NoopFormatter);

try
{
    PythonEngine.Initialize();

    using (var gil = Py.GIL())
    {
        dynamic np = Py.Import("numpy");
        Console.WriteLine(np.cos(np.pi * 2));

        dynamic sin = np.sin;
        Console.WriteLine(sin(5));

        double c = (double)(np.cos(5) + sin(5));
        Console.WriteLine(c);

        dynamic a = np.array(new List<float> { 1, 2, 3 });
        Console.WriteLine(a.dtype);

        dynamic b = np.array(new List<float> { 6, 5, 4 }, dtype: np.int32);
        Console.WriteLine(b.dtype);

        Console.WriteLine(a * b);
    }
}
finally
{
    PythonEngine.Shutdown();
}
