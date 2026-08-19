namespace RaqmiTweaks.App.Models;

public sealed class UwpAppEntry
{
    public string Name { get; set; } = string.Empty;
    public string PackageFamilyName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}
