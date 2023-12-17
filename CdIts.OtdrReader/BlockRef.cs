namespace CdIts.OtdrReader;

internal class BlockRef
{
    public string Name { get; }
    public double Version { get; }
    public uint StartIndex { get; }
    public uint Length { get; }
    public int HeaderLenght => Name.Length + 7;
    public BlockRef(string name, double version, uint startIndex, uint length)
    {
        Name = name.Trim();
        Version = version;
        StartIndex = startIndex;
        Length = length;
    }
}