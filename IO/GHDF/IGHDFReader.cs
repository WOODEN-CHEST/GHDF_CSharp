using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHEngine.IO.GHDF;

public interface IGHDFReader
{
    // Static methods
    static IGHDFReader GetReaderVersion1()
    {
        return new GHDFReaderVersion1();
    }


    // Methods.
    public GHDFCompound Read(Stream stream);
    public GHDFCompound Read(string filePath);
}