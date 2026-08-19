using System.Windows;
using RaqmiTweaks.App.Services;
using RaqmiTweaks.App.ViewModels;

namespace RaqmiTweaks.App;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;

    public MainWindow(TelemetryService telemetryService)
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel(telemetryService);
        DataContext = _viewModel;
        Closed += (_, _) => _viewModel.Dispose();
    }
}