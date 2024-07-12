using Greggs.Products.Models;
using Greggs.Products.Shared;

namespace Greggs.Products.DataAccess;

public class FinanceAccess : IFinanceAccess
{
    private List<CurrencyPair> ExampleCurrencyPairDatabase = new List<CurrencyPair>
    {
        new CurrencyPair
        {
            BaseCurrency = "GBP",
            QuoteCurrency = "EUR",
            ExchangeRate = 1.21m,
            ValidFromDate = new DateTime(2024, 6, 20)
        },
        new CurrencyPair
        {
            BaseCurrency = "GBP",
            QuoteCurrency = "EUR",
            ExchangeRate = 1.11m,
            ValidFromDate = new DateTime(2024, 7, 1)
        }
    };

    public GenericResult<CurrencyPair> GetLatestExchangeRate(string baseCurrencyCode, string quoteCurrencyCode)
    {
        // This represents functionality that in a real world application would look up the correct latest exchange rate from a real database/external service/config etc.
        var latestCurrencyPair = ExampleCurrencyPairDatabase
            .AsQueryable()
            .Where(m => m.BaseCurrency == baseCurrencyCode)
            .Where(m => m.QuoteCurrency == quoteCurrencyCode)
            .OrderByDescending(m => m.ValidFromDate)
            .FirstOrDefault();

        return GenericResult<CurrencyPair>.Ok(latestCurrencyPair);
    }
}
