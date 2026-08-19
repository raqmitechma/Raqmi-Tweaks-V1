namespace RaqmiTweaks.App.Models;

public sealed class HostsEntry
{
    public string Address { get; set; } = string.Empty;
    public string Hostname { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}
