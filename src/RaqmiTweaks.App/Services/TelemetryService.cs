using System.Threading.Channels;
using LibreHardwareMonitor.Hardware;
using RaqmiTweaks.App.Models;

namespace RaqmiTweaks.App.Services;

public sealed class TelemetryService : IDisposable
{
    private readonly Computer _computer;
    private readonly UpdateVisitor _visitor = new();
    private readonly Channel<TelemetrySnapshot> _channel = Channel.CreateBounded<TelemetrySnapshot>(new BoundedChannelOptions(4)
    {
        SingleReader = true,
        SingleWriter = true,
        FullMode = BoundedChannelFullMode.DropOldest,
    });
    private readonly CancellationTokenSource _cts = new();
    private Task? _poller;

    public ChannelReader<TelemetrySnapshot> Snapshots => _channel.Reader;

    public TelemetryService()
    {
        _computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsStorageEnabled = true,
        };

        _computer.Open();
    }

    public void Start()
    {
        if (_poller is not null)
            return;

        _computer.Accept(_visitor);
        _poller = Task.Run(PollLoopAsync, CancellationToken.None);
    }

    public void Stop()
    {
        _cts.Cancel();
    }

    private async Task PollLoopAsync()
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
        try
        {
            while (await timer.WaitForNextTickAsync(_cts.Token))
            {
                var snapshot = Collect();
                _channel.Writer.TryWrite(snapshot);
            }
        }
        catch (OperationCanceledException)
        {
            // Normal shutdown.
        }
        finally
        {
            _channel.Writer.TryComplete();
        }
    }

    private TelemetrySnapshot Collect()
    {
        _computer.Accept(_visitor);

        var cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
        var gpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.GpuNvidia || h.HardwareType == HardwareType.GpuAmd || h.HardwareType == HardwareType.GpuIntel);
        var memory = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Memory);
        var storage = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Storage);

        var cpuLoad = cpu?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name.Contains("CPU Total", StringComparison.OrdinalIgnoreCase))?.Value ?? 0d;
        var gpuLoad = gpu?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name.Contains("GPU Core", StringComparison.OrdinalIgnoreCase))?.Value ?? 0d;
        var cpuTemp = cpu?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name.Contains("CPU Package", StringComparison.OrdinalIgnoreCase))?.Value;
        var gpuTemp = gpu?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name.Contains("GPU Core", StringComparison.OrdinalIgnoreCase))?.Value;
        var memoryLoad = memory?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name.Contains("Memory", StringComparison.OrdinalIgnoreCase))?.Value ?? 0d;
        var diskLoad = storage?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name.Contains("Used", StringComparison.OrdinalIgnoreCase))?.Value ?? 0d;

        long totalRam = 0;
        long usedRam = 0;

        if (memory is not null)
        {
            totalRam = memory.Sensors
                .Where(s => s.SensorType == SensorType.Data && s.Name.Contains("Total", StringComparison.OrdinalIgnoreCase))
                .Select(s => (long)(s.Value ?? 0d))
                .FirstOrDefault();

            usedRam = memory.Sensors
                .Where(s => s.SensorType == SensorType.Data && s.Name.Contains("Used", StringComparison.OrdinalIgnoreCase))
                .Select(s => (long)(s.Value ?? 0d))
                .FirstOrDefault();
        }

        if (totalRam == 0 && usedRam == 0)
        {
            totalRam = 16L * 1024 * 1024 * 1024;
            usedRam = (long)(totalRam * (memoryLoad / 100d));
        }

        return new TelemetrySnapshot(
            CpuLoadPercent: cpuLoad,
            GpuLoadPercent: gpuLoad,
            CpuTempC: cpuTemp,
            GpuTempC: gpuTemp,
            RamUsedBytes: usedRam,
            RamTotalBytes: totalRam,
            DiskActivityPercent: diskLoad,
            Timestamp: DateTimeOffset.UtcNow);
    }

    public void Dispose()
    {
        _cts.Cancel();
        _computer.Close();
        _cts.Dispose();
    }

    private sealed class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer) => computer.Traverse(this);

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (var subHardware in hardware.SubHardware)
            {
                subHardware.Accept(this);
            }
        }

        public void VisitSensor(ISensor sensor) { }

        public void VisitParameter(IParameter parameter) { }
    }
}
