namespace CdIts.OtdrReader;

public interface IWriteableBlock
{
    public double Version { get; }
    public string Name { get; }
    public void Write(BinaryWriter writer);
}