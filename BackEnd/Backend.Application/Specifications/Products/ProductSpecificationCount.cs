using Backend.Application.Domain;
using Backend.Application.Features.Products.Queries.GetProductsByParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Specifications.Products
{
    public class ProductSpecificationCount : Specification<Product>
    {
        public ProductSpecificationCount(GetProductsByParamsQuery @params) : base(

                   c =>
                       (!@params.CategoryId.HasValue || c.Category.Categoryid == @params.CategoryId) &&
                       (!@params.Discontinued.HasValue || c.Discontinued == @params.Discontinued) &&
                (string.IsNullOrEmpty(@params.Search)
                || c.Productname.Contains(@params.Search)
                || c.Category.Categoryname.Contains(@params.Search)
                || c.Unitsinstock.ToString().Contains(@params.Search)
                )
            )
        {
            
        }
    }
}
