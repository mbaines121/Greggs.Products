using Greggs.Products.DataAccess;
using Greggs.Products.Models;
using Greggs.Products.Services;
using Greggs.Products.Shared;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Greggs.Products.UnitTests.ProductServiceTests;

public class ShouldReturnOkWithForeignCurrency
{
    private readonly IProductsService _sut;
    private readonly Mock<IFinancialService> _financialServiceMock;
    private readonly Mock<IDataAccess<Product>> _dataAccessMock;

    public ShouldReturnOkWithForeignCurrency()
    {
        _financialServiceMock = new Mock<IFinancialService>();
        _dataAccessMock = new Mock<IDataAccess<Product>>();

        _sut = new ProductsService(_financialServiceMock.Object, _dataAccessMock.Object);
    }

    [Theory]
    [InlineData("1.27", "1.12", "1.42")]
    [InlineData("1.27", "0", "0")]
    [InlineData("1.27", "0.87", "1.10")]
    [InlineData("0", "0.87", "0")]
    public void With_foreign_currency_returned(string priceInPounds, string exchangeRate, string expectedForeignPrice)
    {
        // Arrange.
        var pageStart = 0;
        var pageSize = 5;
        var currency = "EUR";

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
}
