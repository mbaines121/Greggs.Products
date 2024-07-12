using System;
using System.Collections.Generic;
using Greggs.Products.Services;
using Greggs.Products.Shared;
using Greggs.Products.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductsService _productsService;

    public ProductController(ILogger<ProductController> logger, 
        IProductsService productsService)
    {
        _logger = logger;
        _productsService = productsService;
    }

    /// <summary>
    /// Gets a list of the latest products, with the prices in both GBP and a foreign currency.
    /// </summary>
    /// <param name="pageStart">The product index to start the first page.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <param name="foreignCurrencyCode">Three letter alpha code of the foreign currency. Leave blank if no foreign currency conversion is required.</param>
    /// <returns>Returns a list of ProductViewModel products.</returns>
    /// <response code="200">Returns a list of products. The list may be empty.</response>
    /// <response code="400">Returns a 400 if the pageStart or pageSize are invalid.</response>
    /// <response code="500">Returns a 500 if there was an error returning the products.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Get(int pageStart = 0, int pageSize = 5, string foreignCurrencyCode = "")
    {
        try
        {
            if (pageStart < 0)
            {
                return BadRequest("The page start must be at least zero.");
            }

            if (pageSize < 1)
            {
                return BadRequest("The page size must be at least one item.");
            }

            var result = _productsService.GetProducts(pageStart, pageSize, foreignCurrencyCode);
            if (!result.Success)
            {
                return Ok(new GenericResponse<IEnumerable<ProductViewModel>>(null, result.Message, result.Success));
            }

            return Ok(new GenericResponse<IEnumerable<ProductViewModel>>(result.Object, result.Message, result.Success));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return StatusCode(500, "Oops! We're sorry we weren't able to show you our great products at this time. They will be worth the wait though!");
        }
    }
}