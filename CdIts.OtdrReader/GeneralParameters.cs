namespace CdIts.OtdrReader;

public class GeneralParameters
{
    public readonly string LanguageCode;
    public readonly string CableId;
    public readonly string FiberId;
    public readonly ushort FiberType;
    public readonly ushort WaveLength;
    public readonly string LocationA;
    public readonly string LocationB;
    public readonly string FiberTypeName;
    public readonly string BuildCondition;
    public readonly int UserOffset;
    public readonly int UserOffsetDistance;
    public readonly string Operator;
    public readonly string Comments;

    public GeneralParameters(Span<byte> frame)
    {
        frame.TakeString(out var header)
            .TakeString(out LanguageCode, 2)
            .TakeString(out CableId)
            .TakeString(out FiberId)
            .TakeUShort(out FiberType)
            .TakeUShort(out WaveLength)
            .TakeString(out LocationA)
            .TakeString(out LocationB)
            .TakeString(out FiberTypeName)
            .TakeString(out BuildCondition, 2)
            .TakeInt(out UserOffset)
            .TakeInt(out UserOffsetDistance)
            .TakeString(out Operator)
            .TakeString(out Comments);
    }
}