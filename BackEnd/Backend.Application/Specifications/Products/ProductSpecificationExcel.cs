using Backend.Application.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Specifications.Products
{
    public class ProductSpecificationExcel : Specification<Product>
    {
        public ProductSpecificationExcel(GeneralParams @params) : base(c => c.Discontinued == false)
        {
            //AddOrderByDescending(c => c.CreatedDate!);
        }
    }
}
