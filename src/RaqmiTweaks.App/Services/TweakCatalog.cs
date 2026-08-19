using Microsoft.Win32;
using RaqmiTweaks.App.Models;

namespace RaqmiTweaks.App.Services;

public static class TweakCatalog
{
    public static IReadOnlyList<TweakDefinition> GetDefaults() => new[]
    {
        new TweakDefinition
        {
            Id = "disable-telemetry",
            Title = "Disable Telemetry",
            Category = "Privacy",
            Description = "Turns off Windows diagnostic and ad personalization settings similar to the Win11Debloat privacy baseline.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection",
            RegistryValueName = "AllowTelemetry",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 4,
            EstimatedLatencyReductionMs = 11,
            EstimatedRamSavingsMb = 18,
            IsSelected = true,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-search-history",
            Title = "Disable Search History",
            Category = "Privacy",
            Description = "Disables Search history and recent activity tracking in the Windows shell.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Search",
            RegistryValueName = "HistoryAgeInDays",
            EnabledValue = 0,
            DisabledValue = 90,
            EstimatedFpsGainPercent = 2,
            EstimatedLatencyReductionMs = 7,
            EstimatedRamSavingsMb = 12,
            IsSelected = true,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-copilot",
            Title = "Disable Copilot",
            Category = "AI",
            Description = "Disables Copilot integrations and prompts in the taskbar and app surface.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Policies\\Microsoft\\Windows\\WindowsCopilot",
            RegistryValueName = "TurnOffWindowsCopilot",
            EnabledValue = 1,
            DisabledValue = 0,
            EstimatedFpsGainPercent = 3,
            EstimatedLatencyReductionMs = 9,
            EstimatedRamSavingsMb = 15,
            IsSelected = true,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-chat-taskbar",
            Title = "Disable Chat In Taskbar",
            Category = "Taskbar",
            Description = "Hides the Chat icon from the taskbar and reduces taskbar clutter.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced",
            RegistryValueName = "TaskbarMn",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 2,
            EstimatedLatencyReductionMs = 6,
            EstimatedRamSavingsMb = 8,
            IsSelected = true,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "hide-search-taskbar",
            Title = "Hide Search Taskbar",
            Category = "Taskbar",
            Description = "Removes the search box from the taskbar to create a cleaner shell.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Search",
            RegistryValueName = "SearchboxTaskbarMode",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 1,
            EstimatedLatencyReductionMs = 4,
            EstimatedRamSavingsMb = 6,
            IsSelected = false,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-animations",
            Title = "Disable Animations",
            Category = "Appearance",
            Description = "Disables visual animations to make the desktop feel more responsive.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced",
            RegistryValueName = "DisableAnimation",
            EnabledValue = 1,
            DisabledValue = 0,
            EstimatedFpsGainPercent = 5,
            EstimatedLatencyReductionMs = 13,
            EstimatedRamSavingsMb = 21,
            IsSelected = true,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-transparency",
            Title = "Disable Transparency",
            Category = "Appearance",
            Description = "Disables transparency effects to reduce resource churn and improve clarity.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced",
            RegistryValueName = "UseOLEDTaskbarTransparency",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 3,
            EstimatedLatencyReductionMs = 8,
            EstimatedRamSavingsMb = 11,
            IsSelected = true,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-window-snapping",
            Title = "Disable Window Snapping",
            Category = "Multitasking",
            Description = "Disables Win11 snap assist when the user wants a more traditional tiled workflow.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced",
            RegistryValueName = "SnapAssist",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 2,
            EstimatedLatencyReductionMs = 5,
            EstimatedRamSavingsMb = 7,
            IsSelected = false,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-snap-layouts",
            Title = "Disable Snap Layouts",
            Category = "Multitasking",
            Description = "Disables recommendation overlays when dragging windows to the edges.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced",
            RegistryValueName = "EnableSnapAssistFlyout",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 2,
            EstimatedLatencyReductionMs = 6,
            EstimatedRamSavingsMb = 5,
            IsSelected = false,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-storage-sense",
            Title = "Disable Storage Sense",
            Category = "System",
            Description = "Prevents automatic cleanup and improves predictability for low-storage systems.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\StorageSense",
            RegistryValueName = "StoragePolicy",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 1,
            EstimatedLatencyReductionMs = 4,
            EstimatedRamSavingsMb = 16,
            IsSelected = true,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-fast-startup",
            Title = "Disable Fast Startup",
            Category = "System",
            Description = "Forces a full shutdown so the system behaves more predictably after a tweak session.",
            RegistryHive = RegistryHive.LocalMachine,
            RegistryPath = "System\\CurrentControlSet\\Control\\Session Manager\\Power",
            RegistryValueName = "HiberbootEnabled",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 2,
            EstimatedLatencyReductionMs = 9,
            EstimatedRamSavingsMb = 10,
            IsSelected = false,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-activity-history",
            Title = "Disable Activity History",
            Category = "Privacy",
            Description = "Turns off Windows activity collection that feeds the Start menu and suggestions.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Policies\\Microsoft\\Windows\\System",
            RegistryValueName = "PublishUserActivities",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 1,
            EstimatedLatencyReductionMs = 3,
            EstimatedRamSavingsMb = 9,
            IsSelected = true,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-device-auto-app-download",
            Title = "Disable Device Auto App Download",
            Category = "App Management",
            Description = "Stops automatic companion app installs from being pulled in during device setup.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Device Metadata",
            RegistryValueName = "PreventDeviceMetadataFromBeingUploaded",
            EnabledValue = 1,
            DisabledValue = 0,
            EstimatedFpsGainPercent = 1,
            EstimatedLatencyReductionMs = 2,
            EstimatedRamSavingsMb = 4,
            IsSelected = false,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "disable-notifications",
            Title = "Disable Notifications",
            Category = "Privacy",
            Description = "Reduces notification churn and manual interruptions across the operating system.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\PushNotifications",
            RegistryValueName = "ToastEnabled",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 1,
            EstimatedLatencyReductionMs = 4,
            EstimatedRamSavingsMb = 6,
            IsSelected = false,
            Applied = false
        },
        new TweakDefinition
        {
            Id = "enable-dark-mode",
            Title = "Enable Dark Mode",
            Category = "Appearance",
            Description = "Applies a darker system palette for comfort and aesthetics.",
            RegistryHive = RegistryHive.CurrentUser,
            RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
            RegistryValueName = "AppsUseLightTheme",
            EnabledValue = 0,
            DisabledValue = 1,
            EstimatedFpsGainPercent = 2,
            EstimatedLatencyReductionMs = 5,
            EstimatedRamSavingsMb = 8,
            IsSelected = false,
            Applied = false
        }
    };
}
