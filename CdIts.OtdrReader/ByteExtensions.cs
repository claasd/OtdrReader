using System.Buffers.Binary;
using System.Text;

namespace CdIts.OtdrReader;

public static class ByteExtensions
{
    public static ushort ReadUShort(this Span<byte> data, int offset = 0)
    {
        return BinaryPrimitives.ReadUInt16LittleEndian(data[offset..(offset + 2)]);
    }
    public static short ReadShort(this Span<byte> data, int offset = 0)
    {
        return BinaryPrimitives.ReadInt16LittleEndian(data[offset..(offset + 2)]);
    }

    public static uint ReadUInt(this Span<byte> data, int offset = 0)
    {
        return BinaryPrimitives.ReadUInt32LittleEndian(data[offset..(offset + 4)]);
    }
    public static int ReadInt(this Span<byte> data, int offset = 0)
    {
        return BinaryPrimitives.ReadInt32LittleEndian(data[offset..(offset + 4)]);
    }

    public static Span<byte> TakeString(this Span<byte> data, out string str, int length)
    {
        str = Encoding.UTF8.GetString(data[..length]);
        return data[length..];
    }
    public static Span<byte> TakeString(this Span<byte> data, out string str)
    {
        var index = data.IndexOf((byte)0);
        str = Encoding.UTF8.GetString(data[..index]);
        return data[(index + 1)..];
    }

    public static Span<byte> TakeUInt(this Span<byte> data, out uint value)
    {
        value = data.ReadUInt();
        return data[4..];
    }
    
    public static Span<byte> TakeInt(this Span<byte> data, out int value)
    {
        value = data.ReadInt();
        return data[4..];
    }
    
    public static Span<byte> TakeUShort(this Span<byte> data, out ushort value)
    {
        value = data.ReadUShort();
        return data[2..];
    }
    
    public static Span<byte> TakeShort(this Span<byte> data, out short value)
    {
        value = data.ReadShort();
        return data[2..];
    }
    
    public static Span<byte> TakeUnixTimestamp(this Span<byte> data, out DateTimeOffset timestamp)
    {
        var unixTimestamp = data.ReadUInt();
        timestamp = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        return data[4..];
    }
    
    public static void WriteStringAsByte(this BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value + '\0');
        writer.Write(bytes);
    }
    public static void WriteStringAsByte(this BinaryWriter writer, string value, int len)
    {
        while(value.Length < len)
            value += ' ';
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes);
    }
}