namespace CdIts.OtdrReader;

public class DataPointsBlock : IWriteableBlock
{
    public const string BlockName = "DataPts";
    public List<TraceDataPoints> Traces { get; } = new();

    public DataPointsBlock()
    {
    }
    internal DataPointsBlock(double version, Span<byte> frame)
    {
        Version = version;
        frame = frame.TakeString(out _)
            .TakeUInt(out var numDataPoints)
            .TakeUShort(out var numTraces);
        for (var i = 0; i < numTraces; i++)
        {
            var trace = new TraceDataPoints(frame);
            Traces.Add(trace);
            frame = frame[trace.FrameLength..];
        }
    }

    public double Version { get; } = 1.0;
    public string Name => BlockName;
    public void Write(BinaryWriter writer)
    {
        writer.WriteStringAsByte(Name);
        writer.Write((uint)(Traces.FirstOrDefault()?.DataPoints.Count ?? 0));
        writer.Write((ushort)Traces.Count);
        foreach (var trace in Traces)
        {
            writer.Write((uint)trace.DataPoints.Count);
            writer.Write(trace.ScalingFactor);
            foreach (var dataPoint in trace.DataPoints)
            {
                writer.Write(dataPoint);
            }
        }
    }
}
