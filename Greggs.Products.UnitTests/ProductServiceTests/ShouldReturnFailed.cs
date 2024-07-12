using Greggs.Products.DataAccess;
using Greggs.Products.Models;
using Greggs.Products.Services;
using Greggs.Products.Shared;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.ProductServiceTests;

public class ShouldReturnFailed
{
    private readonly IProductsService _sut;
    private readonly Mock<IFinancialService> _financialServiceMock;
    private readonly Mock<IDataAccess<Product>> _dataAccessMock;

    public ShouldReturnFailed()
    {
        _financialServiceMock = new Mock<IFinancialService>();
        _dataAccessMock = new Mock<IDataAccess<Product>>();

        _sut = new ProductsService(_financialServiceMock.Object, _dataAccessMock.Object);
    }

    [Fact]
    public void With_result_false()
    {
        // Arrange.
        var pageStart = 0;
        var pageSize = 5;

        _financialServiceMock
            .Setup(m => m.GetLatestExchangeRate(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(GenericResult<CurrencyPair>.Failed(string.Empty));

        // Act.
        var result = _sut.GetProducts(pageStart, pageSize, "EUR");

        // Assert.
        Assert.False(result.Success);
    }

    [Fact]
    public void With_result_message()
    {
        // Arrange.
        var pageStart = 0;
        var pageSize = 5;

        const string message = "There was an error retrieving the exchange rate";

        _financialServiceMock
            .Setup(m => m.GetLatestExchangeRate(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(GenericResult<CurrencyPair>.Failed(message));

        // Act.
        var result = _sut.GetProducts(pageStart, pageSize, "EUR");

        // Assert.
        Assert.Equal(message, result.Message);
    }
}
