using Backend.Application.Models.ViewModels;
using Backend.Application.Models.ViewModels.Products;
using Backend.Application.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Products.Queries.GetProductsByParams
{
    public class GetProductsByParamsQuery : GeneralParams, IRequest<PaginationVm<ProductVm>>
    {
  
    }
}
