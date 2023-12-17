namespace CdIts.OtdrReader;

public class TraceDataPoints
{
    public readonly ushort ScalingFactor;
    public IReadOnlyList<ushort> DataPoints { get; }
    public int FrameLength => DataPoints.Count * 2 + 6;

    public TraceDataPoints(Span<byte> frame)
    {
        frame = frame.TakeUInt(out var numDataPoints)
            .TakeUShort(out ScalingFactor);
        var dataPoints = new ushort[numDataPoints];
        for (var i = 0; i < numDataPoints; i++)
        {
            frame = frame.TakeUShort(out var dataPoint);
            dataPoints[i] = dataPoint;
        }

        DataPoints = dataPoints;
    }
}