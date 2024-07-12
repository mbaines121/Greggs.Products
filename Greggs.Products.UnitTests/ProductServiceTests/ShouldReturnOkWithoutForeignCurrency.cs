using Greggs.Products.DataAccess;
using Greggs.Products.Models;
using Greggs.Products.Services;
using Greggs.Products.Shared;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Greggs.Products.UnitTests.ProductServiceTests;

public class ShouldReturnOkWithoutForeignCurrency
{
    private readonly IProductsService _sut;
    private readonly Mock<IFinancialService> _financialServiceMock;
    private readonly Mock<IDataAccess<Product>> _dataAccessMock;

    public ShouldReturnOkWithoutForeignCurrency()
    {
        _financialServiceMock = new Mock<IFinancialService>();
        _dataAccessMock = new Mock<IDataAccess<Product>>();

        _sut = new ProductsService(_financialServiceMock.Object, _dataAccessMock.Object);
    }

    [Theory]
    [InlineData("1.27", "0", "0")]
    [InlineData("0", "0", "0")]
    public void With_foreign_currency_returned(string priceInPounds, string exchangeRate, string expectedForeignPrice)
    {
        // Arrange.
        var pageStart = 0;
        var pageSize = 5;
        var currency = "";

        _financialServiceMock
            .Setup(m => m.GetLatestExchangeRate(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(GenericResult<CurrencyPair>.Ok(new CurrencyPair
            {
                BaseCurrency = "GBP",
                QuoteCurrency = currency,
                ExchangeRate = Convert.ToDecimal(exchangeRate)
            }));

        _dataAccessMock
            .Setup(m => m.List(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(new List<Product>
            {
                new Product
                {
                    Name = "Sausage Roll",
                    PriceInPounds = Convert.ToDecimal(priceInPounds)
                }
            });

        // Act.
        var result = _sut.GetProducts(pageStart, pageSize, currency);

        // Assert.
        var product = result.Object.First();

        Assert.Equal(currency, product.ForeignCurrencyCode);
        Assert.Equal(Convert.ToDecimal(priceInPounds), product.PriceInPounds);
        Assert.Equal(Convert.ToDecimal(expectedForeignPrice), product.PriceInForeignCurrency);
    }

    /// <summary>
    /// Testing the scenario where the financial service does not find an exchange rate and returns null - even though the operation was successful.
    /// </summary>
    [Theory]
    [InlineData("1.27", "0")]
    [InlineData("0", "0")]
    public void With_foreign_currency_returned_null(string priceInPounds, string expectedForeignPrice)
    {
        // Arrange.
        var pageStart = 0;
        var pageSize = 5;
        var currency = "";

        _financialServiceMock
            .Setup(m => m.GetLatestExchangeRate(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(GenericResult<CurrencyPair>.Ok(null));

        _dataAccessMock
            .Setup(m => m.List(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(new List<Product>
            {
                new Product
                {
                    Name = "Sausage Roll",
                    PriceInPounds = Convert.ToDecimal(priceInPounds)
                }
            });

        // Act.
        var result = _sut.GetProducts(pageStart, pageSize, currency);

        // Assert.
        var product = result.Object.First();

        Assert.Equal(currency, product.ForeignCurrencyCode);
        Assert.Equal(Convert.ToDecimal(priceInPounds), product.PriceInPounds);
        Assert.Equal(Convert.ToDecimal(expectedForeignPrice), product.PriceInForeignCurrency);
    }
}
