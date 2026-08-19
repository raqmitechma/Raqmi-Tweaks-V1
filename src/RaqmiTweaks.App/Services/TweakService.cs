using Microsoft.Win32;
using RaqmiTweaks.App.Models;

namespace RaqmiTweaks.App.Services;

public sealed class TweakService
{
    private readonly List<TweakDefinition> _catalog;

    public TweakService()
    {
        _catalog = TweakCatalog.GetDefaults().ToList();
    }

    public IReadOnlyList<TweakDefinition> Catalog => _catalog;

    public void Apply(TweakDefinition tweak, bool enabled)
    {
        var rootKey = RegistryKey.OpenBaseKey(tweak.RegistryHive, RegistryView.Registry64);
        var subKey = rootKey.CreateSubKey(tweak.RegistryPath, true);
        if (subKey is null)
        {
            return;
        }

        var value = enabled ? tweak.EnabledValue : tweak.DisabledValue;
        subKey.SetValue(tweak.RegistryValueName, value, RegistryValueKind.DWord);
        tweak.Applied = enabled;
        subKey.Close();
    }

    public void ApplySelected(IEnumerable<TweakDefinition> tweaks)
    {
        foreach (var tweak in tweaks)
        {
            Apply(tweak, true);
        }
    }

    public void RevertSelected(IEnumerable<TweakDefinition> tweaks)
    {
        foreach (var tweak in tweaks)
        {
            Apply(tweak, false);
        }
    }
}
