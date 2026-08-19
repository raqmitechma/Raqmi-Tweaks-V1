using System.Collections.ObjectModel;
using Microsoft.Win32;
using RaqmiTweaks.App.Models;

namespace RaqmiTweaks.App.Services;

public sealed class StartupManagerService
{
    public IReadOnlyList<StartupEntry> Load()
    {
        var items = new List<StartupEntry>();
        using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

        if (key is null)
        {
            return items;
        }

        foreach (var valueName in key.GetValueNames())
        {
            var value = key.GetValue(valueName);
            items.Add(new StartupEntry
            {
                Name = valueName,
                Command = value?.ToString() ?? string.Empty,
                Enabled = true
            });
        }

        return items;
    }

    public void Toggle(string name, bool enabled)
    {
        using var key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
        if (enabled)
        {
            var current = key?.GetValue(name);
            if (current is null)
            {
                return;
            }
        }
        else
        {
            key?.DeleteValue(name, false);
        }
    }
}
