using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHEngine.IO.GHDF;

public interface IGHDFWriter
{
    // Static methods.
    static IGHDFWriter GetWriterVersion1()
    {
        return new GHDFWriterVersion1();
    }


    // Methods.
    void Write(string filePath, GHDFCompound compound);

    void Write(Stream stream, GHDFCompound compound);
}