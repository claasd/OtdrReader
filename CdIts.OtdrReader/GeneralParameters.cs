namespace CdIts.OtdrReader;

public class GeneralParameters : IWriteableBlock
{
    public const string BlockName  = "GenParams";
    public double Version { get; set; } = 1.0;
    public string LanguageCode = "EN";
    public string CableId = "";
    public string FiberId = "";
    public ushort FiberType = 652;
    public ushort WaveLength = 1550;
    public string LocationA = "";
    public string LocationB = "";
    public string FiberTypeName = "";
    public string BuildCondition = "BC";
    public int UserOffset;
    public int UserOffsetDistance;
    public string Operator = "";
    public string Comments = "";

    public GeneralParameters()
    {
    }

    internal GeneralParameters(double version, Span<byte> frame)
    {
        Version = version;
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
    public string Name => BlockName;
    

    public void Write(BinaryWriter writer)
    {
        writer.WriteStringAsByte(Name);
        writer.WriteStringAsByte(LanguageCode, 2);
        writer.WriteStringAsByte(CableId);
        writer.WriteStringAsByte(FiberId);
        writer.Write(FiberType);
        writer.Write(WaveLength);
        writer.WriteStringAsByte(LocationA);
        writer.WriteStringAsByte(LocationB);
        writer.WriteStringAsByte(FiberTypeName);
        writer.WriteStringAsByte(BuildCondition, 2);
        writer.Write(UserOffset);
        writer.Write(UserOffsetDistance);
        writer.WriteStringAsByte(Operator);
        writer.WriteStringAsByte(Comments);
    }
}