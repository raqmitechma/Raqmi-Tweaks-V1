using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using RaqmiTweaks.App.Models;
using RaqmiTweaks.App.Services;

namespace RaqmiTweaks.App.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    private readonly TelemetryService _telemetryService;

    [ObservableProperty]
    private TelemetrySnapshot _snapshot;

    [ObservableProperty]
    private string _statusText = "Monitoring idle";

    public MainWindowViewModel(TelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
        _telemetryService.Start();

        _ = Task.Run(async () =>
        {
            await foreach (var snapshot in _telemetryService.Snapshots.ReadAllAsync())
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Snapshot = snapshot;
                    StatusText = $"CPU {snapshot.CpuDisplay} · RAM {snapshot.RamDisplay} · Disk {snapshot.DiskDisplay}";
                });
            }
        });
    }

    public void Dispose()
    {
        _telemetryService.Stop();
    }
}
