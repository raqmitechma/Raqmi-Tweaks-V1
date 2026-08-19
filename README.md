# Raqmi-Tweaks-V1

RAQMI TWEEKS is a Windows optimization utility that applies post-installation system tweaks and shows live performance telemetry.

## Build the release

From the repository root, run:

```powershell
powershell -ExecutionPolicy Bypass -File .\setup.ps1
```

This publishes the app in Release mode for `win-x64`, writes the output to `artifacts\release`, and packages it as `artifacts\RaqmiTweaks-Release.zip`.

## Notes

- The app targets Windows and WPF.
- System tweaking actions may require elevation and the app should be run as Administrator when applying registry and startup changes.
- For local development, use the .NET 8 SDK installed on your machine or the user-local SDK under `%USERPROFILE%\.dotnet`.
