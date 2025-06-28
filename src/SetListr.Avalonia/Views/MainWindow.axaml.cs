using Avalonia.Controls;

using SetListr.Avalonia.ViewModels;

namespace SetListr.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = App.Current?.GetRequiredService<MainWindowViewModel>().AssumeNotNull();
    }
}