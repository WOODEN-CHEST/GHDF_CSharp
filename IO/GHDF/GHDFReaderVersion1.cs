using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHEngine.IO.GHDF;

internal class GHDFReaderVersion1 : IGHDFReader
{
    // Private static fields.
    private const byte BOOLEAN_FALSE = 0;
    private const byte BOOLEAN_TRUE = 1;
    private const int VERSION = 1;


    // Private fields.
    private readonly byte[] _signature = { 102, 37, 143, 181, 3, 205, 123, 185, 148, 157, 98, 177, 178, 151, 43, 170 };


    // Private methods.
    private byte ReadByteSafe(Stream stream, string exceptionMsg)
    {
        int Value = stream.ReadByte();
        if (Value == -1)
        {
            throw new GHDFReadException(exceptionMsg ?? "Unexpected end of stream");
        }
        return (byte)Value;
    }

    private byte[] ReadByteArraySafe(Stream stream, int count, string exceptionMsg)
    {
        byte[] ValueArray = new byte[count];
        if (stream.Read(ValueArray) != count)
        {
            throw new GHDFReadException(exceptionMsg ?? "Unexpected end of stream");
        }
        return ValueArray;
    }

    private long Read7BitEncodedInt(Stream stream)
    {
        ulong FinalValue = 0;
        int ReadByteCount = 0;
        const int BITS_PER_BYTE = 7;
        const byte BIT_MASK = 0b0111_1111;
        const byte INDICATING_BIT = 0b1000_0000;

        int CurrentByte;

        do
        {
            CurrentByte = stream.ReadByte();
            if (CurrentByte == -1)
            {
                throw new GHDFReadException($"Unexpected end of stream reading 7bit encoded integer on byte index {ReadByteCount}");
            }
            FinalValue |= ((ulong)(CurrentByte & BIT_MASK) << (BITS_PER_BYTE * ReadByteCount));
            ReadByteCount++;
        } while ((CurrentByte & INDICATING_BIT) != 0);


        return (long)FinalValue;
    }

    private void VerifySignature(Stream stream)
    {
        byte[] ReadBytes = new byte[_signature.Length];
        if ((stream.Read(ReadBytes) < ReadBytes.Length) || !ReadBytes.SequenceEqual(_signature))
        {
            throw new GHDFReadException("Invalid signature, not a GHDF container.");
        }
    }

    private void VerifyVersion(Stream stream)
    {
        long Version = Read7BitEncodedInt(stream);
        if (Version != VERSION)
        {
            throw new GHDFReadException($"Invalid GHDF container version, expected {VERSION}, got {Version}");
        }
    }

    private void VerifyMetadata(Stream stream)
    {
        VerifySignature(stream);
        VerifyVersion(stream);
    }

    private object ReadEntryByType(Stream stream, GHDFType type, bool isArray)
    {
        if (type == GHDFType.UInt8)
        {
            return isArray ? ReadArray(stream, ReadByte) : ReadByte(stream);
        }
        if (type == GHDFType.Int8)
        {
            return isArray ? ReadArray(stream, ReadSByte) : ReadSByte(stream);
        }
        if (type == GHDFType.Int16)
        {
            return isArray ? ReadArray(stream, ReadShort) : ReadShort(stream);
        }
        if (type == GHDFType.UInt16)
        {
            return isArray ? ReadArray(stream, ReadUShort) : ReadUShort(stream);
        }
        if (type == GHDFType.Int32)
        {
            return isArray ? ReadArray(stream, ReadInt) : ReadInt(stream);
        }
        if (type == GHDFType.UInt32)
        {
            return isArray ? ReadArray(stream, ReadUInt) : ReadUInt(stream);
        }
        if (type == GHDFType.Int64)
        {
            return isArray ? ReadArray(stream, ReadLong) : ReadLong(stream);
        }
        if (type == GHDFType.UInt64)
        {
            return isArray ? ReadArray(stream, ReadULong) : ReadULong(stream);
        }
        if (type == GHDFType.Float)
        {
            return isArray ? ReadArray(stream, ReadFloat) : ReadFloat(stream);
        }
        if (type == GHDFType.Double)
        {
            return isArray ? ReadArray(stream, ReadDouble) : ReadDouble(stream);
        }
        if (type == GHDFType.Boolean)
        {
            return isArray ? ReadArray(stream, ReadBool) : ReadBool(stream);
        }
        if (type == GHDFType.String)
        {
            return isArray ? ReadArray(stream, ReadString) : ReadString(stream);
        }
        if (type == GHDFType.Compound)
        {
            return isArray ? ReadArray(stream, ReadCompound) : ReadCompound(stream);
        }
        if (type == GHDFType.EncodedInt)
        {
            return isArray ? ReadArray(stream, ReadEncodedInt) : ReadEncodedInt(stream);
        }
        throw new GHDFReadException($"Invalid entry type: {type} (IsArray: {isArray})");
    }

    private byte ReadByte(Stream stream)
    {
        return ReadByteSafe(stream, "Expected UInt8 value");
    }

    private sbyte ReadSByte(Stream stream)
    {
        return (sbyte)ReadByteSafe(stream, "Expected Int8 value");
    }

    private short ReadShort(Stream stream)
    {
        return BitConverter.ToInt16(ReadByteArraySafe(stream, sizeof(short), "Expected Int16 value"));
    }

    private ushort ReadUShort(Stream stream)
    {
        return BitConverter.ToUInt16(ReadByteArraySafe(stream, sizeof(ushort), "Expected UInt16 value"));
    }

    private int ReadInt(Stream stream)
    {
        return BitConverter.ToInt32(ReadByteArraySafe(stream, sizeof(int), "Expected Int32 value"));
    }

    private uint ReadUInt(Stream stream)
    {
        return BitConverter.ToUInt32(ReadByteArraySafe(stream, sizeof(uint), "Expected Int32 value"));
    }

    private long ReadLong(Stream stream)
    {
        return BitConverter.ToInt64(ReadByteArraySafe(stream, sizeof(long), "Expected Int64 value"));
    }

    private ulong ReadULong(Stream stream)
    {
        return BitConverter.ToUInt64(ReadByteArraySafe(stream, sizeof(ulong), "Expected UInt64 value"));
    }

    private float ReadFloat(Stream stream)
    {
        return BitConverter.ToSingle(ReadByteArraySafe(stream, sizeof(float), "Expected Float value"));
    }

    private double ReadDouble(Stream stream)
    {
        return BitConverter.ToDouble(ReadByteArraySafe(stream, sizeof(double), "Expected Double value"));
    }

    private bool ReadBool(Stream stream)
    {
        byte ReadByte = ReadByteSafe(stream, "Expected boolean value");
        return ReadByte switch
        {
            0 => false,
            1 => true,
            _ => throw new GHDFReadException($"Invalid boolean type value: {ReadByte}")
        };
    }

    private string ReadString(Stream stream)
    {
        int StringLength = (int)Read7BitEncodedInt(stream);
        return Encoding.UTF8.GetString(ReadByteArraySafe(stream, StringLength, "Expected String value"));
    }

    private GHDFEncodedInt ReadEncodedInt(Stream stream)
    {
        return new(Read7BitEncodedInt(stream));
    }

    private T[] ReadArray<T>(Stream stream, Func<Stream, T> readAction)
    {
        int ArrayLength = (int)Read7BitEncodedInt(stream);
        T[] ValueArray = new T[ArrayLength];
        for (int i = 0; i < ArrayLength; i++)
        {
            ValueArray[i] = readAction.Invoke(stream);
        }
        return ValueArray;
    }

    private void ReadEntryIntoCompound(Stream stream, GHDFCompound compound)
    {
        ulong ID = (ulong)Read7BitEncodedInt(stream);
        int TypeByte = stream.ReadByte();
        if (TypeByte == -1)
        {
            throw new GHDFReadException("Expected type of entry, got end of stream");
        }

        int RawTypeByte = TypeByte & (~(byte)GHDFTypeModifier.Array);

        if (!Enum.IsDefined(typeof(GHDFType), RawTypeByte))
        {
            throw new GHDFReadException($"Invalid entry type: {TypeByte}");
        }

        try
        {
            bool IsArray = (TypeByte & (byte)GHDFTypeModifier.Array) != 0;
            compound.Add(ID, ReadEntryByType(stream, (GHDFType)RawTypeByte, IsArray));
        }
        catch (IOException e)
        {
            throw new GHDFReadException($"Failed to read GHDF element with ID {ID} of type {TypeByte}. {e}");
        }
    }

    private GHDFCompound ReadCompound(Stream stream)
    {
        ulong EntryCount = (ulong)Read7BitEncodedInt(stream);
        GHDFCompound Compound = new();

        for (ulong i = 0; i < EntryCount; i++)
        {
            ReadEntryIntoCompound(stream, Compound);
        }

        return Compound;
    }


    // Inherited methods.
    public GHDFCompound Read(Stream stream)
    {
        MemoryStream MemStream = new MemoryStream();
        stream.CopyTo(MemStream);
        MemStream.Position = 0;

        VerifyMetadata(MemStream);

        GHDFCompound Compound = ReadCompound(MemStream);
        if (MemStream.Position < MemStream.Length)
        {
            throw new GHDFReadException("Trailing data in GHDF stream");
        }
        return Compound;
    }

    public GHDFCompound Read(string filePath)
    {
        using Stream ContainerStream = File.OpenRead(filePath);
        return Read(ContainerStream);
    }
}