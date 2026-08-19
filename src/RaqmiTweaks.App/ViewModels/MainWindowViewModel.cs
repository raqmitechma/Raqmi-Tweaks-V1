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
    private readonly StartupManagerService _startupManagerService = new();
    private readonly HostsManagerService _hostsManagerService = new();

    [ObservableProperty]
    private TelemetrySnapshot _snapshot;

    [ObservableProperty]
    private string _statusText = "Monitoring idle";

    [ObservableProperty]
    private string _configSummary = "No configuration loaded.";

    [ObservableProperty]
    private string _exportJson = string.Empty;

    [ObservableProperty]
    private string _backupJson = string.Empty;

    public ObservableCollection<TweakDefinition> TweakRows { get; } = new();
    public ObservableCollection<StartupEntry> StartupEntries { get; } = new();
    public ObservableCollection<HostsEntry> HostsEntries { get; } = new();

    public MainWindowViewModel(TelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
        _tweakService = new TweakService();

        foreach (var tweak in _tweakService.Catalog)
        {
            TweakRows.Add(tweak);
        }

        LoadStartupEntries();
        LoadHostsEntries();
        ValidateExampleConfiguration();

        _telemetryService.Start();

        _ = Task.Run(async () =>
        {
            await foreach (var snapshot in _telemetryService.Snapshots.ReadAllAsync())
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Snapshot = snapshot;
                    var selected = TweakRows.Where(t => t.IsSelected).ToList();
                    var ramFreedMb = selected.Sum(t => t.EstimatedRamSavingsMb);
                    StatusText = $"CPU {snapshot.CpuDisplay} · RAM {snapshot.RamDisplay} · Estimated {ramFreedMb:F0} MB freed";
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

        var estimatedRam = selected.Sum(t => t.EstimatedRamSavingsMb);
        var predictedFps = selected.Sum(t => t.EstimatedFpsGainPercent);
        StatusText = $"Applied {selected.Count} tweak(s) · predicted +{predictedFps:F0}% FPS · ~{estimatedRam:F0} MB RAM freed";
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

    [RelayCommand]
    private void LoadStartupEntries()
    {
        StartupEntries.Clear();
        foreach (var item in _startupManagerService.Load())
        {
            StartupEntries.Add(item);
        }
    }

    [RelayCommand]
    private void LoadHostsEntries()
    {
        HostsEntries.Clear();
        foreach (var item in _hostsManagerService.Load())
        {
            HostsEntries.Add(item);
        }
    }

    [RelayCommand]
    private void ValidateExampleConfiguration()
    {
        var sample = ConfigurationService.BuildDefaultSettingsJson();
        if (ConfigurationService.TryValidateJson(sample, out var error))
        {
            ConfigSummary = "Validated config schema: supported version 1.0";
        }
        else
        {
            ConfigSummary = error;
        }
    }

    [RelayCommand]
    private void ExportConfiguration()
    {
        ExportJson = ConfigurationService.BuildExportJson(TweakRows.Where(t => t.IsSelected));
        ConfigSummary = "Generated an exported configuration payload for deployment.";
    }

    [RelayCommand]
    private void CreateRegistryBackup()
    {
        var selectedIds = TweakRows.Where(t => t.IsSelected).Select(t => t.Id).ToArray();
        BackupJson = RegistryBackupService.BuildJson("CurrentUser:raqmibuild", selectedIds);
        ConfigSummary = "Generated a registry backup payload matching the supported JSON structure.";
    }

    public void Dispose()
    {
        _telemetryService.Stop();
    }
}
