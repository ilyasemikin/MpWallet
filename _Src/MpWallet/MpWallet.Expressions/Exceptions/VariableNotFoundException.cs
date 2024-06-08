using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Expressions.Exceptions;

public sealed class VariableNotFoundException : Exception
{
    public string VariableName { get; }

    public VariableNotFoundException(string name)
        : base($"Variable \"{name}\" not found")
    {
        VariableName = name;
    }
}
