using System.Text;

namespace CdIts.OtdrReader;

public class OtdrReader
{
    private readonly List<BlockRef> _blocks = new();
    private readonly byte[] _data;
    public double Version { get; }

    public string[] GetBlockNames() => _blocks.Select(b => b.Name).ToArray();

    public byte[] GetBlockData(string name, bool removeHeader = false)
    {
        var blockRef = _blocks.Find(b => b.Name == name);
        if (blockRef is null)
            throw new ArgumentException($"File does not contain a block named {name}");
        var span = _data.AsSpan((int)blockRef.StartIndex, (int)blockRef.Length);
        if (removeHeader)
            span = span.TakeString(out _);
        return span.ToArray();
    }

    public byte[] GetChecksum()
    {
        var blockRef = _blocks.Find(b => b.Name == "Cksum");
        if (blockRef is null)
            throw new ArgumentException("File does not contain a checksum block");
        var span = _data.AsSpan((int)blockRef.StartIndex, (int)blockRef.Length);
        var checksum = span.TakeString(out var _)[..2];
        return checksum.ToArray();
    }

    public static OtdrReader ReadByteArray(byte[] data) => new(data);

    public static async ValueTask<OtdrReader> ReadFileAsync(string path)
    {
        var data = await File.ReadAllBytesAsync(path);
        return new OtdrReader(data);
    }

    public static async ValueTask<OtdrReader> ReadStreamAsync(Stream stream)
    {
        var mem = new MemoryStream();
        await stream.CopyToAsync(mem);
        return new OtdrReader(mem.ToArray());
    }

    public string GetChecksumString() => BitConverter.ToString(GetChecksum());

    public GeneralParameters? GetGeneralParameters()
    {
        var blockRef = _blocks.Find(b => b.Name == "GenParams");
        return blockRef is null ? null : new GeneralParameters(_data.AsSpan((int)blockRef.StartIndex, (int)blockRef.Length));
    }

    public SupplierParameters? GetSupplierParameters()
    {
        var blockRef = _blocks.Find(b => b.Name == "SupParams");
        return blockRef is null ? null : new SupplierParameters(_data.AsSpan((int)blockRef.StartIndex, (int)blockRef.Length));
    }

    public FixedParameters? GetFixedParameters()
    {
        var blockRef = _blocks.Find(b => b.Name == "FxdParams");
        return blockRef is null ? null : new FixedParameters(_data.AsSpan((int)blockRef.StartIndex, (int)blockRef.Length));
    }

    public KeyEvents? GetKeyEvents()
    {
        var blockRef = _blocks.Find(b => b.Name == "KeyEvents");
        return blockRef is null ? null : new KeyEvents(_data.AsSpan((int)blockRef.StartIndex, (int)blockRef.Length));
    }

    public IReadOnlyList<TraceDataPoints>? GetDataPoints()
    {
        var blockRef = _blocks.Find(b => b.Name == "DataPts");
        return blockRef is null ? null : new DataPointsBlock(_data.AsSpan((int)blockRef.StartIndex, (int)blockRef.Length)).Traces;
    }

    private OtdrReader(byte[] data)
    {
        _data = data;
        var span = data.AsSpan();
        if (!span[..4].SequenceEqual(Encoding.ASCII.GetBytes("Map\0")))
            throw new ArgumentException("File is not a valid OTDR v2 file");
        Version = span.ReadUShort(4) / 100.0;
        var nextBlockOffset = span.ReadUInt(6);
        var numBlocks = span.ReadUShort(10);
        var nextBlockInfoStart = 12;
        for (var i = 1; i < numBlocks; i++)
        {
            var block = ExtractNextBlock(span[nextBlockInfoStart..], nextBlockOffset);
            _blocks.Add(block);
            nextBlockInfoStart += block.HeaderLenght;
            nextBlockOffset += block.Length;
        }
    }

    private BlockRef ExtractNextBlock(Span<byte> data, uint offset)
    {
        var index = data.IndexOf((byte)0);
        var blockName = Encoding.ASCII.GetString(data[..index]);
        var version = data.ReadUShort(index + 1) / 100.0;
        var numBytes = data.ReadUInt(index + 3);
        return new BlockRef(blockName, version, offset, numBytes);
    }
}