using CatalogService.Application.Commands.ProductCommands.CreateProduct;
using CatalogService.Application.Commands.ProductCommands.UpdateProduct;
using CatalogService.Application.Commands.ProductCommands.DeleteProduct;
using CatalogService.Application.Queries.ProductQueries.GetAllProducts;
using CatalogService.Application.Queries.ProductQueries.GetProductById;
using CatalogService.Application.Queries.ProductQueries.GetProductsByCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpGet("category/{categoryId:guid}")]
    public async Task<IActionResult> GetByCategory(Guid categoryId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductsByCategoryQuery(categoryId), ct);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command, CancellationToken ct)
    {
        if (id != command.Id)
            return BadRequest("ID in URL does not match ID in body.");

        await _mediator.Send(command, ct);
        return Ok(new { message = "Product updated successfully." });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteProductCommand(id), ct);
        return Ok(new { message = "Product deleted successfully." });
    }
}