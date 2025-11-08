using AutoMapper;
using Backend.Application.Contracts.Persistence;
using Backend.Application.Domain;
using Backend.Application.Features.Products.Queries.GetProductsByParams;
using Backend.Application.Helpers;
using Backend.Application.Models.ViewModels;
using Backend.Application.Models.ViewModels.Products;
using Backend.Application.Specifications;
using Backend.Application.Specifications.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Products.Queries.GetProductsById
{
    public class GetProductsByIdHandler : IRequestHandler<GetProductsByIdQuery, ProductVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {      
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        }
        public async Task<ProductVm> Handle(GetProductsByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new Spec(request.ProductId);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpec(spec);
            return _mapper.Map<ProductVm>(product);

        }
        class Spec : Specification<Product>
        {
            public Spec(int id)
                : base(x => x.Productid == id)
            {
                AddInclude("Category");
            }
        }
    }
}
