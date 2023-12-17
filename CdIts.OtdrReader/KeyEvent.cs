namespace CdIts.OtdrReader;

public class KeyEvent
{
    public readonly ushort EventNumber;
    public readonly uint TimeOfTravel;
    public readonly short Slope;
    public readonly short SpliceLoss;
    public readonly int ReflectionLoss;
    public readonly string EventType;
    public readonly uint EndOfPreviousEvent;
    public readonly uint BeginningOfCurrentEvent;
    public readonly uint EndOfCurrentEvent;
    public readonly uint BeginningOfNextEvent;
    public readonly uint PeakPointInCurrentEvent;
    public readonly string Comment;

    public KeyEvent(Span<byte> data)
    {
        data.TakeUShort(out EventNumber)
            .TakeUInt(out TimeOfTravel)
            .TakeShort(out Slope)
            .TakeShort(out SpliceLoss)
            .TakeInt(out ReflectionLoss)
            .TakeString(out EventType, 8)
            .TakeUInt(out EndOfPreviousEvent)
            .TakeUInt(out BeginningOfCurrentEvent)
            .TakeUInt(out EndOfCurrentEvent)
            .TakeUInt(out BeginningOfNextEvent)
            .TakeUInt(out PeakPointInCurrentEvent)
            .TakeString(out Comment);
    }

    public int FrameLength => 43 + Comment.Length;
}