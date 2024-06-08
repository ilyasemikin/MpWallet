using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Currencies;

public sealed record CurrencyRatio(Currency Antecedent, Currency Consequent);
