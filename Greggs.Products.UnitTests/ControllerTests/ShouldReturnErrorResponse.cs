using Greggs.Products.Api.Controllers;
using Greggs.Products.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.ControllerTests;

public class ShouldReturnErrorResponse
{
    private readonly ProductController _sut;

    public ShouldReturnErrorResponse()
    {
        var loggerMock = new Mock<ILogger<ProductController>>();
        var productServiceMock = new Mock<IProductsService>();

        productServiceMock
            .Setup(m => m.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .Throws(new System.Exception("A transient error."));

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
    public void With_response_500()
    {
        // Arrange.
        var pageStart = 0;
        var pageSize = 5;

        // Act.
        var response = _sut.Get(pageStart, pageSize);

        // Assert.
        var result = (ObjectResult)response;

        Assert.Equal(500, result.StatusCode);
    }
}
