namespace CdIts.OtdrReader;

public class KeyEvents
{
    public readonly int TotalLoss;
    public readonly int FiberStartPosition;
    public readonly uint FiberLength;
    public readonly ushort OpticalReturnLoss;
    public IReadOnlyList<KeyEvent> Events { get; }

    public KeyEvents(Span<byte> frame)
    {
        frame = frame.TakeString(out _) // Key Events
            .TakeShort(out var numEvents); // Key Events
        var events = new KeyEvent[numEvents];
        for (var i = 0; i < numEvents; i++)
        {
            var ev = new KeyEvent(frame);
            events[i] = ev;
            frame = frame[ev.FrameLength..];
        }

        Events = events;
        frame.TakeInt(out TotalLoss)
            .TakeInt(out FiberStartPosition)
            .TakeUInt(out FiberLength)
            .TakeUShort(out OpticalReturnLoss);
    }
}