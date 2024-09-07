using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHEngine.IO.GHDF;

public class GHDFEntryException : Exception
{
    public GHDFEntryException(string? message) : base(message) { }
}