namespace RaqmiTweaks.App.Models;

public sealed class StartupEntry
{
    public string Name { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}
