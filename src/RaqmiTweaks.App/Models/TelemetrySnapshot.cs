namespace RaqmiTweaks.App.Models;

public readonly record struct TelemetrySnapshot(
    double CpuLoadPercent,
    double GpuLoadPercent,
    double? CpuTempC,
    double? GpuTempC,
    long RamUsedBytes,
    long RamTotalBytes,
    double DiskActivityPercent,
    DateTimeOffset Timestamp)
{
    public string CpuDisplay => $"{CpuLoadPercent:0}%";
    public string RamDisplay => $"{RamUsedBytes / 1024d / 1024d / 1024d:0.0} GB";
    public string DiskDisplay => $"{DiskActivityPercent:0}%";
}
