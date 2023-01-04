using SilkWPF.OpenGL.Sample;
using System.Windows;

namespace SilkWPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < 4; i++)
        {
            await Task.Delay(1000);
            GL.Children.Add(new Materials());
        }
    }
}
