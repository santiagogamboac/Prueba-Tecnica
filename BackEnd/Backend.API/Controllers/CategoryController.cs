using Backend.Application.Features.Categories.Commands;
using Backend.Application.Features.Categories.Queries;
using Backend.Application.Features.Products.Commands.BulkCreateRandomProducts;
using Backend.Application.Features.Products.Queries.GetProductsByParams;
using Backend.Application.Models.Common;
using Backend.Application.Models.ViewModels;
using Backend.Application.Models.ViewModels.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
    #if !DEBUG
    [Authorize]
    #endif
    public class CategoryController : ControllerBase
    {
        private IMediator _mediator { get; set; }
        public CategoryController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryVm>), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<CategoryVm>>> GetCategories()
        => Ok(await _mediator.Send(new GetCategoriesQuery()));

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GeneralResponse>> CreateCategory([FromBody] AddCategoryCommand command)
        => Ok(await _mediator.Send(command));
    }
}
