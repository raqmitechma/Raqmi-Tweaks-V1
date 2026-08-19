using System.IO;
using System.Linq;
using RaqmiTweaks.App.Models;

namespace RaqmiTweaks.App.Services;

public sealed class HostsManagerService
{
    public IReadOnlyList<HostsEntry> Load()
    {
        var filePath = Path.Combine(Environment.SystemDirectory, "drivers", "etc", "hosts");
        var entries = new List<HostsEntry>();

        if (!File.Exists(filePath))
        {
            return entries;
        }

        foreach (var line in File.ReadAllLines(filePath))
        {
            var trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith('#'))
            {
                continue;
            }

            var parts = trimmed.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                continue;
            }

            entries.Add(new HostsEntry
            {
                Address = parts[0],
                Hostname = string.Join(' ', parts.Skip(1)),
                Enabled = true
            });
        }

        return entries;
    }
}
