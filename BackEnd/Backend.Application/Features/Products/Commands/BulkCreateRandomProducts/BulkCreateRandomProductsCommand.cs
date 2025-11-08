using Backend.Application.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Products.Commands.BulkCreateRandomProducts
{
    public class BulkCreateRandomProductsCommand : IRequest<GeneralResponse>
    {
        public int Count { get; set; } = 100000;
    }
}
