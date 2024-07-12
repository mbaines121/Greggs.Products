using Greggs.Products.DataAccess;
using Greggs.Products.Services;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.FinancialServiceTests;

public class ShouldReturnEmpty
{
    private readonly IFinancialService _sut;

    public ShouldReturnEmpty()
    {
        var financeAccessMock = new Mock<IFinanceAccess>();

        _sut = new FinancialService(financeAccessMock.Object);
    }

    [Theory]
    [InlineData("GBP", "EU")]
    [InlineData("", "EUR")]
    [InlineData("", null)]
    [InlineData(null, "EUR")]
    [InlineData("EUR", "EUR")]
    public void With_success_false(string baseCurrency, string quoteCurrency)
    {
        // Act.
        var result = _sut.GetLatestExchangeRate(baseCurrency, quoteCurrency);

        // Assert.
        Assert.False(result.Success);
    }

    [Theory]
    [InlineData("GBP", "EU")]
    [InlineData("", "EUR")]
    [InlineData("", null)]
    [InlineData(null, "EUR")]
    [InlineData("EUR", "EUR")]
    public void With_object_null(string baseCurrency, string quoteCurrency)
    {
        // Act.
        var result = _sut.GetLatestExchangeRate(baseCurrency, quoteCurrency);

        // Assert.
        Assert.Null(result.Object);
    }
}
