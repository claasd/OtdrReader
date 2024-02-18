using CdIts.OtdrReader;

namespace CdIts.OtdrWriter;

public class OtdrWriter
{
    public double Version { get; set; }
    private readonly List<IWriteableBlock> _blocks = new();

    public OtdrWriter(double version = 1.0)
    {
        Version = version;
    }

    /// <summary>
    /// add a generic or custom block. The name will be prepended to the data, if the block already contains the block name, set addHeader to false
    /// </summary>
    public void AddBlock(string name, byte[] data, double version = 1.0, bool addHeader = true)
    {
        using var memoryStream = new MemoryStream();
        using (var writer = new BinaryWriter(memoryStream))
        {
            if (addHeader)
                writer.WriteStringAsByte(name + '\0');
            writer.Write(data);
        }

        AddBlock(new WrittenBlock(name, version, data));
    }

    /// <summary>
    /// add one of the predefined blocks such as GeneralParameters, FixedParameters, DataPoints, etc
    /// </summary>
    public void AddBlock(IWriteableBlock block) => _blocks.Add(block);

    public void ToStream(Stream s)
    {
        using var writer = new BinaryWriter(s);
        writer.WriteStringAsByte("Map");
        writer.Write(ToVersion(Version));
        var blockData = _blocks.Select(b => new WrittenBlock(b)).ToList();
        var headerBlock = CreateHeaderBlock(blockData);
        writer.Write((uint)headerBlock.Length + 12);
        writer.Write((ushort)(_blocks.Count + 1));
        writer.Write(headerBlock);
        foreach (var block in blockData)
        {
            writer.Write(block.Data);
        }
    }

    public byte[] ToArray()
    {
        using var ms = new MemoryStream();
        ToStream(ms);
        return ms.ToArray();
    }

    private static ushort ToVersion(double version) => (ushort)(version * 100);

    private static byte[] CreateHeaderBlock(List<WrittenBlock> blockData)
    {
        using var ms = new MemoryStream();
        using (var writer = new BinaryWriter(ms))
        {
            foreach (var block in blockData)
            {
                writer.WriteStringAsByte(block.Name);
                writer.Write(ToVersion(block.Version));
                writer.Write(block.Length);
            }
        }

        return ms.ToArray();
    }
}