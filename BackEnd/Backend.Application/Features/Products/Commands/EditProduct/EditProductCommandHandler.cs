using AutoMapper;
using Backend.Application.Contracts.Persistence;
using Backend.Application.Domain;
using Backend.Application.Models.Common;
using Backend.Application.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Products.Commands.EditProduct
{
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand, GeneralResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(EditProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.Id);
            if (product == null)
                return new GeneralResponse(false, $"No se encontró el producto con Id {command.Id}");

            product.Productname = command.ProductName;
            product.Unitprice = command.UnitPrice;
            product.Unitsinstock = command.UnitsInStock;
            product.Discontinued = command.Discontinued;
            product.Categoryid = command.CategoryId;
            await _unitOfWork.Repository<Product>().UpdateAsync(product);

            return new GeneralResponse(true, $"El Producto fue actualizado exitosamente");
        }
    }
}
