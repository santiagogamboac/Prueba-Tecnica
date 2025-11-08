using Backend.Application.Features.Products.Commands.AddProduct;
using Backend.Application.Features.Products.Commands.BulkCreateRandomProducts;
using Backend.Application.Features.Products.Commands.DeleteProduct;
using Backend.Application.Features.Products.Commands.EditProduct;
using Backend.Application.Features.Products.Queries.GetProductsById;
using Backend.Application.Features.Products.Queries.GetProductsByParams;
using Backend.Application.Models.Common;
using Backend.Application.Models.ViewModels;
using Backend.Application.Models.ViewModels.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend.API.Controllers
{
    [Route("api/product")]
    [ApiController]
    [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
    #if !DEBUG
    [Authorize]
    #endif
    public class ProductsController : ControllerBase
    {
        private IMediator _mediator { get; set; }
        public ProductsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProductVm>> GetProductById(int Id)
        => Ok(await _mediator.Send(new GetProductsByIdQuery {ProductId =Id }));

        [HttpGet]
        [ProducesResponseType(typeof(PaginationVm<ProductVm>), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PaginationVm<ProductVm>>> GetProducts([FromQuery] GetProductsByParamsQuery query)
        => Ok(await _mediator.Send(query));

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GeneralResponse>> CreateProducts([FromBody]AddProductCommand command)
        => Ok(await _mediator.Send(command));


        [HttpPost("bulk")]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GeneralResponse>> CreateRandomProducts()
        => Ok(await _mediator.Send(new BulkCreateRandomProductsCommand()));


        [HttpPut("{id}")]
        public async Task<ActionResult<GeneralResponse>> UpdateProduct(int id, EditProductCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GeneralResponse>> DeleteProduct(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCommand { ProductId = id }));
        }     
    }
}
