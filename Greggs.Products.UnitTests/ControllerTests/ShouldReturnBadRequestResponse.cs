using Greggs.Products.Api.Controllers;
using Greggs.Products.Services;
using Greggs.Products.Shared;
using Greggs.Products.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Greggs.Products.UnitTests.ControllerTests;

public class ShouldReturnBadRequestResponse
{
    private readonly ProductController _sut;

    public ShouldReturnBadRequestResponse()
    {
        var loggerMock = new Mock<ILogger<ProductController>>();
        var productServiceMock = new Mock<IProductsService>();

        productServiceMock
            .Setup(m => m.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .Returns(GenericResult<IEnumerable<ProductViewModel>>.Ok(Enumerable.Empty<ProductViewModel>()));

        _sut = new ProductController(loggerMock.Object, productServiceMock.Object);

    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 5)]
    public void With_valid_response(int pageStart, int pageSize)
    {
        // Act.
        var response = _sut.Get(pageStart, pageSize);

        // Assert.
        Assert.NotNull(response);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 5)]
    public void With_response_400(int pageStart, int pageSize)
    {
        // Act.
        var response = _sut.Get(pageStart, pageSize);

        // Assert.
        var result = (ObjectResult)response;

        Assert.Equal(400, result.StatusCode);
    }
}
