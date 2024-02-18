namespace CdIts.OtdrReader;

public class FixedParameters : IWriteableBlock
{
    public const string BlockName = "FxdParams";
    public List<PulseWidthEntry> PulseWidthEntries { get; } = new();
    public DateTimeOffset Timestamp;
    public string Units = "mt";
    public ushort WaveLength;
    public int AcquisitionOffset;
    public int AcquisitionOffsetDistance;
    public uint IndexOfRefraction;
    public ushort BackscatteringCoefficient;
    public uint NumberOfAverages;
    public ushort AveragingTime;
    public uint Range;
    public int AcquisitionRangeDistance;
    public int FrontPAnelOffset;
    public ushort NoiseFloorLevel;
    public short NoiseFloorScalingFactor;
    public ushort PowerOffsetFirstPoint;
    public ushort LossThreshold;
    public ushort ReflectionThreshold;
    public ushort EndOfTransmitionThreshold;
    public string TraceType = "ST";
    public int X1;
    public int Y1;
    public int X2;
    public int Y2;

    public FixedParameters()
    {
    }

    internal FixedParameters(double version, Span<byte> frame)
    {
        Version = version;
        frame = frame.TakeString(out var _)
            .TakeUnixTimestamp(out Timestamp)
            .TakeString(out Units, 2)
            .TakeUShort(out WaveLength)
            .TakeInt(out AcquisitionOffset)
            .TakeInt(out AcquisitionOffsetDistance)
            .TakeUShort(out var numPulses);
        for (var i = 0; i < numPulses; i++)
        {
            frame = frame.TakeUShort(out var pulseWidth)
                .TakeUInt(out var sampleSpacing)
                .TakeUInt(out var numDataPoints);
            PulseWidthEntries.Add(new PulseWidthEntry(pulseWidth, sampleSpacing, numDataPoints));
        }

        frame.TakeUInt(out IndexOfRefraction)
            .TakeUShort(out BackscatteringCoefficient)
            .TakeUInt(out NumberOfAverages)
            .TakeUShort(out AveragingTime)
            .TakeUInt(out Range)
            .TakeInt(out AcquisitionRangeDistance)
            .TakeInt(out FrontPAnelOffset)
            .TakeUShort(out NoiseFloorLevel)
            .TakeShort(out NoiseFloorScalingFactor)
            .TakeUShort(out PowerOffsetFirstPoint)
            .TakeUShort(out LossThreshold)
            .TakeUShort(out ReflectionThreshold)
            .TakeUShort(out EndOfTransmitionThreshold)
            .TakeString(out TraceType, 2)
            .TakeInt(out X1)
            .TakeInt(out Y1)
            .TakeInt(out X2)
            .TakeInt(out Y2);
    }

    public double Version { get; } = 1.0;
    public string Name => BlockName;

    public void Write(BinaryWriter writer)
    {
        writer.WriteStringAsByte(Name);
        writer.Write((uint)Timestamp.ToUnixTimeSeconds());
        writer.WriteStringAsByte(Units, 2);
        writer.Write(WaveLength);
        writer.Write(AcquisitionOffset);
        writer.Write(AcquisitionOffsetDistance);
        writer.Write((ushort)PulseWidthEntries.Count);
        foreach (var pulseWidthEntry in PulseWidthEntries)
        {
            writer.Write(pulseWidthEntry.PulseWidth);
            writer.Write(pulseWidthEntry.SampleSpacing);
            writer.Write(pulseWidthEntry.NumDataPoints);
        }

        writer.Write(IndexOfRefraction);
        writer.Write(BackscatteringCoefficient);
        writer.Write(NumberOfAverages);
        writer.Write(AveragingTime);
        writer.Write(Range);
        writer.Write(AcquisitionRangeDistance);
        writer.Write(FrontPAnelOffset);
        writer.Write(NoiseFloorLevel);
        writer.Write(NoiseFloorScalingFactor);
        writer.Write(PowerOffsetFirstPoint);
        writer.Write(LossThreshold);
        writer.Write(ReflectionThreshold);
        writer.Write(EndOfTransmitionThreshold);
        writer.WriteStringAsByte(TraceType, 2);
        writer.Write(X1);
        writer.Write(Y1);
        writer.Write(X2);
        writer.Write(Y2);
    }
}