using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Application.Features.Products.Commands.Create;
using ProductManager.Application.Features.Products.Commands.Delete;
using ProductManager.Application.Features.Products.Commands.Update;
using ProductManager.Application.Features.Products.Queries.GetAll;
using ProductManager.Application.Features.Products.Queries.GetById;

namespace ProductManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]
        [Route("/getall")]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = "Name",
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? name = null,
            [FromQuery] string? description = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] decimal? minQuantity = null,
            [FromQuery] decimal? maxQuantity = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(pageNumber, 
                pageSize, 
                sortColumn, 
                sortDirection, 
                name, 
                description, 
                isActive, 
                minQuantity, 
                maxQuantity,
                minPrice, 
                maxPrice), 
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id, CancellationToken cancellationToken = default)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);

            if (product == null)
                return NotFound($"Not found product with id : {id}");

            return Ok(product);
        }

        [HttpPost]
        [Route("/create")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var newProductId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(CreateProduct), new { id = newProductId }, newProductId);
        }


        [HttpDelete]
        [Route("/delete")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPut]
        [Route("/update")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
