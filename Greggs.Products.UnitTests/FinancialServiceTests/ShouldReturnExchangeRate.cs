using Greggs.Products.DataAccess;
using Greggs.Products.Models;
using Greggs.Products.Services;
using Greggs.Products.Shared;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.FinancialServiceTests;

public class ShouldReturnExchangeRate
{
    private readonly IFinancialService _sut;

    public ShouldReturnExchangeRate()
    {
        var financeAccessMock = new Mock<IFinanceAccess>();

        financeAccessMock
            .Setup(m => m.GetLatestExchangeRate(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(GenericResult<CurrencyPair>.Ok(new CurrencyPair()));

        _sut = new FinancialService(financeAccessMock.Object);
    }

    [Theory]
    [InlineData("GBP", "EUR")]
    public void With_success_true(string baseCurrency, string quoteCurrency)
    {
        // Act.
        var result = _sut.GetLatestExchangeRate(baseCurrency, quoteCurrency);

        // Assert.
        Assert.True(result.Success);
    }
}
