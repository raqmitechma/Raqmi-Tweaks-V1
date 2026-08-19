using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RaqmiTweaks.App.Models;
using RaqmiTweaks.App.Services;

namespace RaqmiTweaks.App.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    private readonly TelemetryService _telemetryService;
    private readonly TweakService _tweakService;

    [ObservableProperty]
    private TelemetrySnapshot _snapshot;

    [ObservableProperty]
    private string _statusText = "Monitoring idle";

    public ObservableCollection<TweakDefinition> TweakRows { get; } = new();

    public MainWindowViewModel(TelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
        _tweakService = new TweakService();

        foreach (var tweak in _tweakService.Catalog)
        {
            TweakRows.Add(tweak);
        }

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

    [RelayCommand]
    private void ApplySelectedTweaks()
    {
        var selected = TweakRows.Where(t => t.IsSelected).ToList();
        _tweakService.ApplySelected(selected);
        foreach (var tweak in selected)
        {
            tweak.Applied = true;
        }
        StatusText = $"Applied {selected.Count} tweak(s) from the Win11Debloat catalog.";
    }

    [RelayCommand]
    private void RevertSelectedTweaks()
    {
        var selected = TweakRows.Where(t => t.IsSelected).ToList();
        _tweakService.RevertSelected(selected);
        foreach (var tweak in selected)
        {
            tweak.Applied = false;
        }
        StatusText = $"Reverted {selected.Count} tweak(s).";
    }

    public void Dispose()
    {
        _telemetryService.Stop();
    }
}
