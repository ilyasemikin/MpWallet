using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Expressions.Exceptions;

public sealed class FunctionNotFoundException : Exception
{
    public string FunctionName { get; }

    public FunctionNotFoundException(string name)
        : base($"Function \"{name}\" not found")
    {
        FunctionName = name;
    }
}
