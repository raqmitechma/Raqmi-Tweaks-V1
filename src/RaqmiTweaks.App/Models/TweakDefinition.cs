using Microsoft.Win32;

namespace RaqmiTweaks.App.Models;

public sealed class TweakDefinition
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public RegistryHive RegistryHive { get; init; } = RegistryHive.CurrentUser;
    public string RegistryPath { get; init; } = string.Empty;
    public string RegistryValueName { get; init; } = string.Empty;
    public int EnabledValue { get; init; }
    public int DisabledValue { get; init; }
    public bool IsSelected { get; set; }
    public bool Applied { get; set; }
}
