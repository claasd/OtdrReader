namespace CdIts.OtdrReader;

public class SupplierParameters
{
    public readonly string SupplierName;
    public readonly string OtdrName;
    public readonly string OtdrSerialNumber;
    public readonly string ModuleName;
    public readonly string ModuleSerialNumber;
    public readonly string SoftwareVersion;
    public readonly string Other;

    public SupplierParameters(Span<byte> frame)
    {
        frame.TakeString(out _)
            .TakeString(out SupplierName)
            .TakeString(out OtdrName)
            .TakeString(out OtdrSerialNumber)
            .TakeString(out ModuleName)
            .TakeString(out ModuleSerialNumber)
            .TakeString(out SoftwareVersion)
            .TakeString(out Other);
    }
}