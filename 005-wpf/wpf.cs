#:sdk Microsoft.NET.Sdk
#:property OutputType=WinExe
#:property TargetFramework=net10.0-windows
#:property PublishAot=False
#:property UseWPF=True
#:property UseWindowsForms=False
#:package CommunityToolkit.Mvvm@8.4.0

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

// https://github.com/dotnet/winforms/issues/5071#issuecomment-908789632
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

var countText = new TextBlock
{
    FontSize = 24,
    Margin = new Thickness(8),
    HorizontalAlignment = HorizontalAlignment.Center,
};
countText.SetBinding(TextBlock.TextProperty, new Binding(nameof(CounterViewModel.Count)));

var incButton = new Button { Content = "Increment (+1)", Margin = new Thickness(8), };
incButton.SetBinding(Button.CommandProperty, new Binding(nameof(CounterViewModel.IncrementCountCommand)));

var decButton = new Button { Content = "Decrement (-1)", Margin = new Thickness(8), };
decButton.SetBinding(Button.CommandProperty, new Binding(nameof(CounterViewModel.DecrementCountCommand)));

var root = new StackPanel { Margin = new Thickness(16) };
root.Children.Add(countText);
root.Children.Add(incButton);
root.Children.Add(decButton);

var window = new Window
{
    Title = $"Hello, World! - {Thread.CurrentThread.GetApartmentState()}",
    Width = 320,
    Height = 240,
    Content = root,
    DataContext = new CounterViewModel(),
    WindowStartupLocation = WindowStartupLocation.CenterScreen,
};

var app = new Application
{
    ShutdownMode = ShutdownMode.OnMainWindowClose,
};
app.DispatcherUnhandledException += (_, e) =>
{
    MessageBox.Show(e.Exception.ToString(), "Unhandled", MessageBoxButton.OK, MessageBoxImage.Error);
    e.Handled = true;
};
app.Run(window);

public sealed partial class CounterViewModel : ObservableObject
{
    [ObservableProperty]
    private int _count = 0;

    [RelayCommand]
    private void IncrementCount()
        => Count++;

    [RelayCommand]
    private void DecrementCount()
        => Count--;
}