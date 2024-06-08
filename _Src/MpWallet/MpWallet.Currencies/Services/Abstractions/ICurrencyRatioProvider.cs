using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Currencies.Services.Abstractions;

public interface ICurrencyRatioProvider
{
    bool TryGet(CurrencyRatio ratio, [NotNullWhen(true)] out decimal? value);
}
