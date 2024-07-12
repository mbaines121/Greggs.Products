using Greggs.Products.DataAccess;
using Greggs.Products.Models;
using Greggs.Products.Shared;
using Greggs.Products.ViewModels;

namespace Greggs.Products.Services
{
    public interface IProductsService
    {
        GenericResult<IEnumerable<ProductViewModel>> GetProducts(int pageStart = 0, int pageSize = 5, string foreignCurrency = "");
    }

    public class ProductsService : IProductsService
    {
        private readonly IFinancialService _financialService;
        private readonly IDataAccess<Product> _dataAccess;

        public ProductsService(IFinancialService financialService, 
            IDataAccess<Product> dataAccess)
        {
            _financialService = financialService;
            _dataAccess = dataAccess;
        }

        public GenericResult<IEnumerable<ProductViewModel>> GetProducts(int pageStart = 0, int pageSize = 5, string foreignCurrencyCode = "")
        {
            var financialResult = _financialService.GetLatestExchangeRate("GBP", foreignCurrencyCode);
            if (!financialResult.Success)
            {
                return GenericResult<IEnumerable<ProductViewModel>>.Failed(financialResult.Message);
            }

            var productsResult = _dataAccess.List(pageStart, pageSize);

            var productViewModels = productsResult.Select(m => new ProductViewModel
            {
                Name = m.Name,
                PriceInPounds = m.PriceInPounds,
                ForeignCurrencyCode = foreignCurrencyCode,
                PriceInForeignCurrency = CalculateForeignPrice(m.PriceInPounds, financialResult.Object?.ExchangeRate ?? 0m)
            });

            return GenericResult<IEnumerable<ProductViewModel>>.Ok("Mmmm, our super tasty products!", productViewModels);
        }

        private decimal CalculateForeignPrice(decimal priceInPounds, decimal exchangeRate)
        {
            return Math.Round(priceInPounds * exchangeRate, 2);
        }
    }
}
