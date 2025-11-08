using Backend.Application.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand: IRequest<GeneralResponse>
    {
        public int ProductId {get; set;}
    }
}
