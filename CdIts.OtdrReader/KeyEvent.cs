namespace CdIts.OtdrReader;

public class KeyEvent
{
    public ushort EventNumber;
    public uint TimeOfTravel;
    public short Slope;
    public short SpliceLoss;
    public int ReflectionLoss;
    public string EventType = "";
    public uint EndOfPreviousEvent;
    public uint BeginningOfCurrentEvent;
    public uint EndOfCurrentEvent;
    public uint BeginningOfNextEvent;
    public uint PeakPointInCurrentEvent;
    public string Comment = "";

    public KeyEvent()
    {
        
    }
    internal KeyEvent(Span<byte> data)
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

    internal void Write(BinaryWriter writer)
    {
        writer.Write(EventNumber);
        writer.Write(TimeOfTravel);
        writer.Write(Slope);
        writer.Write(SpliceLoss);
        writer.Write(ReflectionLoss);
        writer.WriteStringAsByte(EventType, 8);
        writer.Write(EndOfPreviousEvent);
        writer.Write(BeginningOfCurrentEvent);
        writer.Write(EndOfCurrentEvent);
        writer.Write(BeginningOfNextEvent);
        writer.Write(PeakPointInCurrentEvent);
        writer.WriteStringAsByte(Comment);
    }
}