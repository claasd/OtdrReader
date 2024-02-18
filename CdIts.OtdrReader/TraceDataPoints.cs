namespace CdIts.OtdrReader;

public class TraceDataPoints
{
    public ushort ScalingFactor = 1;
    public List<ushort> DataPoints { get; } = new();
    public int FrameLength => DataPoints.Count * 2 + 6;

    public TraceDataPoints()
    {
    }
    
    internal TraceDataPoints(Span<byte> frame)
    {
        frame = frame.TakeUInt(out var numDataPoints)
            .TakeUShort(out ScalingFactor);
        for (var i = 0; i < numDataPoints; i++)
        {
            frame = frame.TakeUShort(out var dataPoint);
            DataPoints.Add(dataPoint);
        }
    }
}