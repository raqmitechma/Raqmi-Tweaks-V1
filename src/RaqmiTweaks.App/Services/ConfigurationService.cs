using System.Text.Json;
using RaqmiTweaks.App.Models;

namespace RaqmiTweaks.App.Services;

public static class ConfigurationService
{
    public const string SupportedVersion = "1.0";

    public static bool TryValidateJson(string json, out string error)
    {
        error = string.Empty;

        try
        {
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            if (!root.TryGetProperty("Version", out var versionElement) || versionElement.ValueKind != JsonValueKind.String)
            {
                error = "Configuration is missing a valid Version field.";
                return false;
            }

            if (versionElement.GetString() != SupportedVersion)
            {
                error = $"Unsupported config version '{versionElement.GetString()}'. Supported version is {SupportedVersion}.";
                return false;
            }

            if (root.TryGetProperty("Name", out var nameElement) && nameElement.ValueKind == JsonValueKind.String && string.IsNullOrWhiteSpace(nameElement.GetString()))
            {
                error = "Configuration Name cannot be empty.";
                return false;
            }

            if (root.TryGetProperty("Settings", out var settingsElement) && settingsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var setting in settingsElement.EnumerateArray())
                {
                    if (!setting.TryGetProperty("Name", out var name) || name.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(name.GetString()))
                    {
                        error = "Each setting entry must contain a non-empty Name.";
                        return false;
                    }
                }
            }

            return true;
        }
        catch (JsonException ex)
        {
            error = $"Invalid JSON: {ex.Message}";
            return false;
        }
    }

    public static string BuildDefaultSettingsJson()
    {
        var settings = new object[]
        {
            new { Name = "Supported", Value = true },
            new { Name = "DisableModernStandbyNetworking", Value = true },
            new { Name = "Unknown", Value = true }
        };

        var payload = new
        {
            Version = SupportedVersion,
            Settings = settings
        };

        return JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
    }

    public static string BuildExportJson(IEnumerable<TweakDefinition> tweaks, bool skipRegistryBackup = false)
    {
        var selected = tweaks.Where(t => t.IsSelected).Select(t => new { Name = t.Id, Value = true }).ToArray();
        var deployment = new object[]
        {
            new { Name = "UserSelectionIndex", Value = 0 },
            new { Name = "AppRemovalScopeIndex", Value = 0 },
            new { Name = "CreateRestorePoint", Value = true },
            new { Name = "SkipRegistryBackup", Value = skipRegistryBackup },
            new { Name = "RestartExplorer", Value = false }
        };

        var payload = new
        {
            Version = SupportedVersion,
            Deployment = deployment,
            Tweaks = selected
        };

        return JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
    }
}
