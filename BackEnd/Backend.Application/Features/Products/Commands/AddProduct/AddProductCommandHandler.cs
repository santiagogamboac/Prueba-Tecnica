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

namespace Backend.Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, GeneralResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(
            productName: request.ProductName,
            supplierId: request.SupplierId,
            categoryId: request.CategoryId,
            quantityPerUnit: request.QuantityPerUnit,
            unitPrice: request.UnitPrice,
            unitsInStock: request.UnitsInStock,
            unitsOnOrder: 0,
            reorderLevel: 0,
            discontinued: request.Discontinued
        );
            var newProduct = _mapper.Map<Product>(product);

            await _unitOfWork.Repository<Product>().AddAsync(newProduct);//, $"Se creo un nuevo Producto con nombre denominado: {request.Name}", "Productos");

            return new GeneralResponse(true, $"El Producto fue creado exitosamente");
        }
    }
}
