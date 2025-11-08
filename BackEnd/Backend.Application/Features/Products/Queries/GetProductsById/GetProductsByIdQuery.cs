using Backend.Application.Models.ViewModels.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Products.Queries.GetProductsById
{
    public class GetProductsByIdQuery : IRequest<ProductVm>
    {
        public int ProductId { get; set; }   
    }
}
