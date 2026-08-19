using System.Windows;
using RaqmiTweaks.App.Services;

namespace RaqmiTweaks.App;

public partial class App : Application
{
    private TelemetryService? _telemetryService;
    private MainWindow? _mainWindow;

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        _telemetryService = new TelemetryService();
        _mainWindow = new MainWindow(_telemetryService);
        _mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _mainWindow?.Close();
        _telemetryService?.Dispose();
        base.OnExit(e);
    }
}

