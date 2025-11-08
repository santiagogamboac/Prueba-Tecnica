using Backend.Application.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommand : IRequest<GeneralResponse>
    {
        public string ProductName { get; set; }
        public int SupplierId { get; set; } = 1; // Si por ahora no usas supplier puedes dejar default
        public int CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool Discontinued { get; set; }
    }
}
