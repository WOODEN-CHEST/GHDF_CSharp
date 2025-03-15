using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHEngine.IO.GHDF;

public class GDHFReadVersionMismatchException : GHDFReadException
{
    // Fields.
    public int ExpectedVersion { get; private init; }
    public int FoundVersion { get; private init; }

    public GDHFReadVersionMismatchException(string? message, int expectedVersion, int foundVersion) : base(message)
    {
        ExpectedVersion = expectedVersion;
        FoundVersion = foundVersion;
    }
}