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
    private long Read7BitEncodedInt(Stream stream)
    {
        ulong FinalValue = 0;
        int ReadByteCount = 0;
        const int BITS_PER_BYTE = 7;

        int CurrentByte;

        do
        {
            CurrentByte = stream.ReadByte();
            if (CurrentByte == -1)
            {
                throw new GHDFReadException($"Unexpected end of stream reading 7bit encoded integer on byte index {ReadByteCount}");
            }
            FinalValue |= (ulong)(CurrentByte & (~(byte)GHDFTypeModifier.Array)) << (BITS_PER_BYTE * ReadByteCount);
        } while ((CurrentByte & (byte)GHDFTypeModifier.Array) != 0);


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

    private GHDFCompound ReadCompound(Stream stream)
    {

    }


    // Inherited methods.
    public GHDFCompound Read(Stream stream)
    {
        MemoryStream MemStream = new MemoryStream();
        stream.CopyTo(MemStream);
        return ReadCompound(MemStream);
    }

    public GHDFCompound Read(string filePath)
    {
        using Stream ContainerStream = File.OpenRead(filePath);
        return Read(ContainerStream);
    }
}