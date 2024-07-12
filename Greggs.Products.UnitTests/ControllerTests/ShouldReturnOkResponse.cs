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

public class ShouldReturnOkResponse
{
    private readonly ProductController _sut;

    public ShouldReturnOkResponse()
    {
        var loggerMock = new Mock<ILogger<ProductController>>();
        var productServiceMock = new Mock<IProductsService>();

        productServiceMock
            .Setup(m => m.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .Returns(GenericResult<IEnumerable<ProductViewModel>>.Ok(Enumerable.Empty<ProductViewModel>()));

        _sut = new ProductController(loggerMock.Object, productServiceMock.Object);

    }

    [Fact]
    public void With_valid_response()
    {
        // Arrange.
        var pageStart = 0;
        var pageSize = 5;

        // Act.
        var response = _sut.Get(pageStart, pageSize);

        // Assert.
        Assert.NotNull(response);
    }

    [Fact]
    public void With_response_200()
    {
        // Arrange.
        var pageStart = 0;
        var pageSize = 5;

        // Act.
        var response = _sut.Get(pageStart, pageSize);

        // Assert.
        var result = (ObjectResult)response;

        Assert.Equal(200, result.StatusCode);
    }
}
