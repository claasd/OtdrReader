namespace CdIts.OtdrReader;

public class KeyEvents : IWriteableBlock
{
    public const string BlockName = "KeyEvents";
    public int TotalLoss;
    public int FiberStartPosition;
    public uint FiberLength;
    public ushort OpticalReturnLoss;
    public List<KeyEvent> Events { get; } = new();

    public KeyEvents()
    {
    }

    internal KeyEvents(double version, Span<byte> frame)
    {
        Version = version;
        frame = frame.TakeString(out _) // Key Events
            .TakeShort(out var numEvents); // Key Events
        for (var i = 0; i < numEvents; i++)
        {
            var ev = new KeyEvent(frame);
            Events.Add(ev);
            frame = frame[ev.FrameLength..];
        }
        frame.TakeInt(out TotalLoss)
            .TakeInt(out FiberStartPosition)
            .TakeUInt(out FiberLength)
            .TakeUShort(out OpticalReturnLoss);
    }

    public double Version { get; } = 1.0;
    public string Name => BlockName;
    public void Write(BinaryWriter writer)
    {
        writer.WriteStringAsByte(Name);
        writer.Write((short)Events.Count);
        foreach (var ev in Events)
        {
            ev.Write(writer);
        }

        writer.Write(TotalLoss);
        writer.Write(FiberStartPosition);
        writer.Write(FiberLength);
        writer.Write(OpticalReturnLoss);
    }
}