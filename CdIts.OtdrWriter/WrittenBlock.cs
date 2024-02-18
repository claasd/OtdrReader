using CdIts.OtdrReader;

namespace CdIts.OtdrWriter;

internal class WrittenBlock : IWriteableBlock
{
    public string Name { get; }
    public byte[] Data { get; }
    public double Version { get; }
    public uint Length => (uint)Data.Length;
    public void Write(BinaryWriter writer) => writer.Write(Data);

    internal WrittenBlock(IWriteableBlock block)
    {
        Name = block.Name;
        Version = block.Version;
        using var ms = new MemoryStream();

        using (var writer = new BinaryWriter(ms))
            block.Write(writer);
        Data = ms.ToArray();
    }

    internal WrittenBlock(string name, double version, byte[] data)
    {
        Name = name;
        Version = version;
        Data = data;
    }
}