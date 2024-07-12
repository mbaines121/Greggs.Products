using Greggs.Products.Models;
using Greggs.Products.Shared;

namespace Greggs.Products.DataAccess;

public interface IFinanceAccess
{
    GenericResult<CurrencyPair> GetLatestExchangeRate(string baseCurrencyCode, string quoteCurrencyCode);
}
