using MpWallet.Expressions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MpWallet.Collections.Immutable.Abstractions;

namespace MpWallet.Expressions.Context.Variables;

public record Variable(string Name, Expression Expression) : IImmutableItem;
