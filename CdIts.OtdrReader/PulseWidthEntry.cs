namespace CdIts.OtdrReader;

public class PulseWidthEntry
{
    public readonly ushort PulseWidth;
    public readonly uint SampleSpacing;
    public readonly uint NumDataPoints;

    public PulseWidthEntry(ushort pulseWidth, uint sampleSpacing, uint numDataPoints)
    {
        PulseWidth = pulseWidth;
        SampleSpacing = sampleSpacing;
        NumDataPoints = numDataPoints;
    }
}