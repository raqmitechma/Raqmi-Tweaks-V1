using System.Text.Json;

namespace RaqmiTweaks.App.Services;

public static class RegistryBackupService
{
    public static string BuildJson(string target, IEnumerable<string> selectedFeatures)
    {
        var payload = new
        {
            Version = "1.0",
            BackupType = "RegistryState",
            Target = target,
            SelectedFeatures = selectedFeatures.ToArray(),
            SelectedUndoFeatures = Array.Empty<string>(),
            RegistryKeys = Array.Empty<object>(),
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = "RAQMI Tweaks"
        };

        return JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
    }
}
