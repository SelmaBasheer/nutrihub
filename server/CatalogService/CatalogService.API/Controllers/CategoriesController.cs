using CatalogService.Application.Commands.CategoryCommands.CreateCategory;
using CatalogService.Application.Commands.CategoryCommands.DeleteCategory;
using CatalogService.Application.Commands.CategoryCommands.UpdateCategory;
using CatalogService.Application.Queries.CategoryQueries.GetAllCategories;
using CatalogService.Application.Queries.CategoryQueries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery(), ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id), ct);  
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken ct)
        {
            var id = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetCategoryById), new { id }, new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommand command, CancellationToken ct)
        {
            if (id != command.Id)
                return BadRequest("ID in URL does not match ID in body.");

            await _mediator.Send(command, ct);
            return Ok(new { message = "Category updated successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteCategoryCommand(id), ct);
            return Ok(new { message = "Category deleted successfully." });
        }
    }
}
