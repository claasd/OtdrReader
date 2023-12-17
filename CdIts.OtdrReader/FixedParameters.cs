namespace CdIts.OtdrReader;

public class FixedParameters
{
    public IReadOnlyList<PulseWidthEntry> PulseWidthEntries { get; }
    public readonly DateTimeOffset Timestamp;
    public readonly string Unit;
    public readonly ushort WaveLength;
    public readonly int AcquisitionOffset;
    public readonly int AcquisitionOffsetDistance;
    public readonly uint IndexOfRefraction;
    public readonly ushort BackscatteringCoefficient;
    public readonly uint NumberOfAverages;
    public readonly ushort AveragingTime;
    public readonly uint Range;
    public readonly int AcquisitionRangeDistance;
    public readonly int FrontPAnelOffset;
    public readonly ushort NoiseFloorLevel;
    public readonly short NoiseFloorScalingFactor;
    public readonly ushort PowerOffsetFirstPoint;
    public readonly ushort LossThreshold;
    public readonly ushort ReflectionThreshold;
    public readonly ushort EndOfTransmitionThreshold;
    public readonly string TraceType;
    public readonly int X1;
    public readonly int Y1;
    public readonly int X2;
    public readonly int Y2;

    public FixedParameters(Span<byte> frame)
    {
        frame = frame.TakeString(out var _)
            .TakeUnixTimestamp(out Timestamp)
            .TakeString(out Unit, 2)
            .TakeUShort(out WaveLength)
            .TakeInt(out AcquisitionOffset)
            .TakeInt(out AcquisitionOffsetDistance)
            .TakeUShort(out var numPulses);
        var pulseWidthEntries = new List<PulseWidthEntry>();
        for (var i = 0; i < numPulses; i++)
        {
            frame = frame.TakeUShort(out var pulseWidth)
                .TakeUInt(out var sampleSpacing)
                .TakeUInt(out var numDataPoints);
            pulseWidthEntries.Add(new PulseWidthEntry(pulseWidth, sampleSpacing, numDataPoints));
        }

        PulseWidthEntries = pulseWidthEntries.ToArray();

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
}