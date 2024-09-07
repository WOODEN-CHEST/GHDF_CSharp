using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHEngine.IO.GHDF;

public class GHDFReadException : IOException
{
    public GHDFReadException(string? message) : base(message) { }
}