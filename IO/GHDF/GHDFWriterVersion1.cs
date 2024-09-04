using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHEngine.IO.GHDF;

internal class GHDFWriterVersion1 : IGHDFWriter
{
    // Private static fields.
    private const byte BOOLEAN_FALSE = 0;
    private const byte BOOLEAN_TRUE = 1;
    private const int VERSION = 1;
    private const string FILE_EXTENSION = ".ghdf";


    // Private fields.
    private readonly byte[] _signature = { 102, 37, 143, 181, 3, 205, 123, 185, 148, 157, 98, 177, 178, 151, 43, 170 };


    // Private methods.
    private void WriteSignature(Stream stream)
    {
        stream.Write(_signature);
    }

    private void WriteVersion(Stream stream)
    {
        Write7BitEncodedInt(stream, VERSION);
    }

    private void WriteMetaInfo(Stream stream)
    {
        WriteSignature(stream);
        WriteVersion(stream);
    }

    private void Write7BitEncodedInt(Stream stream, long value)
    {
        const ulong MAX_SINGLE_BYTE_VALUE = 127;
        const ulong ONE_BYTE_MASK = 0xff;

        ulong CurrentValue = (ulong)value;
        do
        {
            ulong LongValueToWrite = CurrentValue;
            if (CurrentValue > MAX_SINGLE_BYTE_VALUE)
            {
                LongValueToWrite |= (ulong)GHDFTypeModifier.Array;
            }
            byte ByteToWrite = (byte)(LongValueToWrite & ONE_BYTE_MASK);

            stream.WriteByte(ByteToWrite);

            CurrentValue >>= 7;
        } while (CurrentValue != 0);
    }

    private byte[] GetAsLittleEndian(byte[] array)
    {
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }
        return array;
    }

    private void WriteSingleEntryValue(Stream stream, object value)
    {
        if (value is byte)
        {
            WriteByte(stream, (byte)value);
        }
        else if (value is sbyte)
        {
            WriteSByte(stream, (sbyte)value);
        }
        else if (value is short)
        {
            WriteShort(stream, (short)value);
        }
        else if (value is ushort)
        {
            WriteUShort(stream, (ushort)value);
        }
        else if (value is int)
        {
            WriteInt(stream, (int)value);
        }
        else if (value is uint)
        {
            WriteUInt(stream, (uint)value);
        }
        else if (value is long)
        {
            WriteLong(stream, (long)value);
        }
        else if (value is ulong)
        {
            WriteULong(stream, (ulong)value);
        }
        else if (value is float)
        {
            WriteFloat(stream, (float)value);
        }
        else if (value is double)
        {
            WriteDouble(stream, (double)value);
        }
        else if (value is bool)
        {
            WriteBool(stream, (bool)value);
        }
        else if (value is string)
        {
            WriteString(stream, (string)value);
        }
        else if (value is GHDFCompound)
        {
            WriteCompound(stream, (GHDFCompound)value);
        }
        else if (value is GHDFEncodedInt)
        {
            WriteEncodedInt(stream, (GHDFEncodedInt)value);
        }
        else if (value is byte[])
        {
            WriteArray(stream, (byte[])value, WriteByte);
        }
        else if (value is sbyte[])
        {
            WriteArray(stream, (sbyte[])value, WriteSByte);
        }
        else if (value is short[])
        {
            WriteArray(stream, (short[])value, WriteShort);
        }
        else if (value is ushort[])
        {
            WriteArray(stream, (ushort[])value, WriteUShort);
        }
        else if (value is int[])
        {
            WriteArray(stream, (int[])value, WriteInt);
        }
        else if (value is uint[])
        {
            WriteArray(stream, (uint[])value, WriteUInt);
        }
        else if (value is long[])
        {
            WriteArray(stream, (long[])value, WriteLong);
        }
        else if (value is ulong[])
        {
            WriteArray(stream, (ulong[])value, WriteULong);
        }
        else if (value is float[])
        {
            WriteArray(stream, (float[])value, WriteFloat);
        }
        else if (value is double[])
        {
            WriteArray(stream, (double[])value, WriteDouble);
        }
        else if (value is bool[])
        {
            WriteArray(stream, (bool[])value, WriteBool);
        }
        else if (value is string[])
        {
            WriteArray(stream, (string[])value, WriteString);
        }
        else if (value is GHDFCompound[])
        {
            WriteArray(stream, (GHDFCompound[])value, WriteCompound);
        }
        else if (value is GHDFEncodedInt[])
        {
            WriteArray(stream, (GHDFEncodedInt[])value, WriteEncodedInt);
        }
        else
        {
            throw new GHDFWriteException($"Invalid type for value: {value?.GetType().FullName}");
        }
    }

    private byte GetTypeOfObjectAsByte(object value)
    {
        if (value is byte)
        {
            return (byte)GHDFType.UInt8;
        }
        if (value is sbyte)
        {
            return (byte)GHDFType.Int8;
        }
        if (value is short)
        {
            return (byte)GHDFType.Int16;
        }
        if (value is ushort)
        {
            return (byte)GHDFType.UInt16;
        }
        if (value is int)
        {
            return (byte)GHDFType.Int32;
        }
        if (value is uint)
        {
            return (byte)GHDFType.UInt32;
        }
        if (value is long)
        {
            return (byte)GHDFType.Int64;
        }
        if (value is ulong)
        {
            return (byte)GHDFType.UInt64;
        }
        if (value is float)
        {
            return (byte)GHDFType.Float;
        }
        if (value is double)
        {
            return (byte)GHDFType.Double;
        }
        if (value is string)
        {
            return (byte)GHDFType.String;
        }
        if (value is bool)
        {
            return (byte)GHDFType.Boolean;
        }
        if (value is GHDFCompound)
        {
            return (byte)GHDFType.Compound;
        }
        if (value is GHDFEncodedInt)
        {
            return (byte)GHDFType.EncodedInt;
        }

        if (value is byte[])
        {
            return (byte)GHDFType.UInt8 | (byte)GHDFTypeModifier.Array;
        }
        if (value is sbyte[])
        {
            return (byte)GHDFType.Int8 | (byte)GHDFTypeModifier.Array;
        }
        if (value is short[])
        {
            return (byte)GHDFType.Int16 | (byte)GHDFTypeModifier.Array;
        }
        if (value is ushort[])
        {
            return (byte)GHDFType.UInt16 | (byte)GHDFTypeModifier.Array;
        }
        if (value is int[])
        {
            return (byte)GHDFType.Int32 | (byte)GHDFTypeModifier.Array;
        }
        if (value is uint[])
        {
            return (byte)GHDFType.UInt32 | (byte)GHDFTypeModifier.Array;
        }
        if (value is long[])
        {
            return (byte)GHDFType.Int64 | (byte)GHDFTypeModifier.Array;
        }
        if (value is ulong[])
        {
            return (byte)GHDFType.UInt64 | (byte)GHDFTypeModifier.Array;
        }
        if (value is float[])
        {
            return (byte)GHDFType.Float | (byte)GHDFTypeModifier.Array;
        }
        if (value is double[])
        {
            return (byte)GHDFType.Double | (byte)GHDFTypeModifier.Array;
        }
        if (value is string[])
        {
            return (byte)GHDFType.String | (byte)GHDFTypeModifier.Array;
        }
        if (value is bool[])
        {
            return (byte)GHDFType.Boolean | (byte)GHDFTypeModifier.Array;
        }
        if (value is GHDFCompound[])
        {
            return (byte)GHDFType.Compound | (byte)GHDFTypeModifier.Array;
        }
        if (value is GHDFEncodedInt[])
        {
            return (byte)GHDFType.EncodedInt | (byte)GHDFTypeModifier.Array;
        }

        throw new GHDFWriteException($"Couldn't convert object type to byte, invalid type: {value?.GetType().FullName}");
    }

    private void WriteByte(Stream stream, byte value)
    {
        stream.WriteByte(value);
    }

    private void WriteSByte(Stream stream, sbyte value)
    {
        stream.WriteByte((byte)value);
    }

    private void WriteShort(Stream stream, short value)
    {
        stream.Write(GetAsLittleEndian(BitConverter.GetBytes(value)));
    }

    private void WriteUShort(Stream stream, ushort value)
    {
        stream.Write(GetAsLittleEndian(BitConverter.GetBytes(value)));
    }

    private void WriteInt(Stream stream, int value)
    {
        stream.Write(GetAsLittleEndian(BitConverter.GetBytes(value)));
    }

    private void WriteUInt(Stream stream, uint value)
    {
        stream.Write(GetAsLittleEndian(BitConverter.GetBytes(value)));
    }

    private void WriteLong(Stream stream, long value)
    {
        stream.Write(GetAsLittleEndian(BitConverter.GetBytes(value)));
    }

    private void WriteULong(Stream stream, ulong value)
    {
        stream.Write(GetAsLittleEndian(BitConverter.GetBytes(value)));
    }

    private void WriteFloat(Stream stream, float value)
    {
        stream.Write(GetAsLittleEndian(BitConverter.GetBytes(value)));
    }

    private void WriteDouble(Stream stream, double value)
    {
        stream.Write(GetAsLittleEndian(BitConverter.GetBytes(value)));
    }

    private void WriteBool(Stream stream, bool value)
    {
        stream.WriteByte(value ? BOOLEAN_TRUE : BOOLEAN_FALSE);
    }

    private void WriteString(Stream stream, string value)
    {
        Write7BitEncodedInt(stream, value.Length);
        stream.Write(Encoding.UTF8.GetBytes(value));
    }

    private void WriteCompound(Stream stream, GHDFCompound compound)
    {
        Write7BitEncodedInt(stream, compound.Count);
        foreach (KeyValuePair<ulong, object> Entry in compound)
        {
            if (Entry.Key == 0)
            {
                throw new GHDFWriteException("Invalid entry with ID of 0");
            }
            try
            {
                Write7BitEncodedInt(stream, (long)Entry.Key);
                stream.WriteByte(GetTypeOfObjectAsByte(Entry.Value));
                WriteSingleEntryValue(stream, Entry.Value);
            }
            catch (GHDFWriteException e)
            {
                throw new GHDFWriteException($"Failed to write entry with ID {Entry.Key} in compound: {{{e.Message}}}");
            }
        }
    }
    private void WriteEncodedInt(Stream stream, GHDFEncodedInt value)
    {
        Write7BitEncodedInt(stream, value.Value);
    }

    private void WriteArray<T>(Stream stream, T[] array, Action<Stream, T> writeAction)
    {
        Write7BitEncodedInt(stream, array.Length);
        foreach (T Item in array)
        {
            writeAction.Invoke(stream, Item);
        }
    }

    // Inherited methods.
    public void Write(string filePath, GHDFCompound compound)
    {
        ArgumentNullException.ThrowIfNull(nameof(filePath));
        string ModifiedPath = (Path.ChangeExtension(filePath, FILE_EXTENSION));

        if (Directory.Exists(ModifiedPath))
        {
            throw new GHDFWriteException($"Given path \"{compound}\" points to a directory");
        }

        File.Delete(ModifiedPath);
        using Stream DataStream = File.OpenWrite(ModifiedPath);
        Write(DataStream, compound);
    }

    public void Write(Stream stream, GHDFCompound compound)
    {
        ArgumentNullException.ThrowIfNull(compound, nameof(compound));
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        Stream MemStream = new MemoryStream();
        WriteMetaInfo(MemStream);
        WriteCompound(MemStream, compound);

        MemStream.Position = 0;
        MemStream.CopyTo(stream);
    }
}