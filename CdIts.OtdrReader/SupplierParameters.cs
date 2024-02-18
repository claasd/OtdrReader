namespace CdIts.OtdrReader;

public class SupplierParameters : IWriteableBlock
{
    public const string BlockName = "SupParameters";
    public string SupplierName = "";
    public string OtdrName = "";
    public string OtdrSerialNumber = "";
    public string ModuleName = "";
    public string ModuleSerialNumber = "";
    public string SoftwareVersion = "";
    public string Other = "";

    public SupplierParameters()
    {
    }

    internal SupplierParameters(double version, Span<byte> frame)
    {
        Version = version;
        frame.TakeString(out _)
            .TakeString(out SupplierName)
            .TakeString(out OtdrName)
            .TakeString(out OtdrSerialNumber)
            .TakeString(out ModuleName)
            .TakeString(out ModuleSerialNumber)
            .TakeString(out SoftwareVersion)
            .TakeString(out Other);
    }

    public double Version { get; } = 1.0;
    public string Name => BlockName;

    public void Write(BinaryWriter writer)
    {
        writer.WriteStringAsByte(Name);
        writer.WriteStringAsByte(SupplierName);
        writer.WriteStringAsByte(OtdrName);
        writer.WriteStringAsByte(OtdrSerialNumber);
        writer.WriteStringAsByte(ModuleName);
        writer.WriteStringAsByte(ModuleSerialNumber);
        writer.WriteStringAsByte(SoftwareVersion);
        writer.WriteStringAsByte(Other);
    }
}