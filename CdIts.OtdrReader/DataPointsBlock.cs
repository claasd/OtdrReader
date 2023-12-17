namespace CdIts.OtdrReader;

public class DataPointsBlock
{
    public IReadOnlyList<TraceDataPoints>? Traces { get; }
    public DataPointsBlock(Span<byte> frame)
    {
        frame = frame.TakeString(out _)
            .TakeUInt(out var numDataPoints)
            .TakeUShort(out var numTraces);
        var traces = new TraceDataPoints[numTraces];
        for (var i = 0; i < numTraces; i++)
        {
            var trace = new TraceDataPoints(frame);
            traces[i] = trace;
            frame = frame[trace.FrameLength..];
        }

        Traces = traces;
    }

    
}