using MpWallet.Expressions.Abstractions;
using MpWallet.Collections.Immutable.Abstractions;

namespace MpWallet.Expressions.Context.Variables;

public record Variable(string Name, Expression Expression) : IImmutableItem;
