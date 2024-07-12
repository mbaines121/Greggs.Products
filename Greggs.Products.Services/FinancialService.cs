using Greggs.Products.DataAccess;
using Greggs.Products.Models;
using Greggs.Products.Shared;
using System.Text.RegularExpressions;

namespace Greggs.Products.Services
{
    public interface IFinancialService
    {
        GenericResult<CurrencyPair> GetLatestExchangeRate(string baseCurrencyCode, string quoteCurrencyCode);
    }

    public class FinancialService : IFinancialService
    {
        private readonly IFinanceAccess _financeAccess;

        public FinancialService(IFinanceAccess financeAccess)
        {
            _financeAccess = financeAccess;
        }

        /// <summary>
        /// Validates the currency codes and gets the latest matching exchange rate.
        /// </summary>
        /// <param name="baseCurrencyCode">The currency alpha code we want to convert from.</param>
        /// <param name="quoteCurrencyCode">The currency alpha code we want to convert to.</param>
        /// <returns>Returns the latest exchange rate if found.</returns>
        public GenericResult<CurrencyPair> GetLatestExchangeRate(string baseCurrencyCode, string quoteCurrencyCode)
        {
            var alphaMatch = new Regex(@"^[A-Za-z]{3}$");

            if (string.IsNullOrEmpty(baseCurrencyCode) || !alphaMatch.IsMatch(baseCurrencyCode))
            {
                return GenericResult<CurrencyPair>.Failed($"The provided base currency '{baseCurrencyCode}' needs to be a three character alpha currency code.");
            }

            if (string.IsNullOrWhiteSpace(quoteCurrencyCode))
            {
                return GenericResult<CurrencyPair>.Ok(new CurrencyPair
                {
                    BaseCurrency = baseCurrencyCode,
                    QuoteCurrency = quoteCurrencyCode,
                    ExchangeRate = 0m
                });
            }

            if (!alphaMatch.IsMatch(quoteCurrencyCode))
            {
                return GenericResult<CurrencyPair>.Failed($"The provided quote currency '{quoteCurrencyCode}' needs to be a three character alpha currency code.");
            }

            if (baseCurrencyCode.ToUpper() == quoteCurrencyCode.ToUpper())
            {
                return GenericResult<CurrencyPair>.Failed($"Unable to convert the base currency '{baseCurrencyCode}' to the quote currency '{quoteCurrencyCode}' as they are the same.");
            }

            return _financeAccess.GetLatestExchangeRate(baseCurrencyCode, quoteCurrencyCode);
        }
    }
}
