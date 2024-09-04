using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHEngine.IO.GHDF;

public enum GHDFType
{
    None = 0,
    UInt8 = 1,
    Int8 = 2,
    UInt16 = 3,
    Int16 = 4,
    UInt32 = 5,
    Int32 = 6,
    UInt64 = 7,
    Int64 = 8,
    Float = 9,
    Double = 10,
    Boolean = 11,
    String = 12,
    Compound = 13,
    EncodedInt = 14
}