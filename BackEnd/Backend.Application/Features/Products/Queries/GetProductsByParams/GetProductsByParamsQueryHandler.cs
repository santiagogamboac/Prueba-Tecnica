using AutoMapper;
using Backend.Application.Contracts.Persistence;
using Backend.Application.Domain;
using Backend.Application.Helpers;
using Backend.Application.Models.ViewModels;
using Backend.Application.Models.ViewModels.Products;
using Backend.Application.Specifications.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Products.Queries.GetProductsByParams
{
    public class GetProductsByParamsQueryHandler : IRequestHandler<GetProductsByParamsQuery, PaginationVm<ProductVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationHelper _paginationHelper;

        public GetProductsByParamsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _paginationHelper = new PaginationHelper(mapper);
        }

        public async Task<PaginationVm<ProductVm>> Handle(GetProductsByParamsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductSpecification(request);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpec(spec);
            var specCount = new ProductSpecificationCount(request);
            var total = await _unitOfWork.Repository<Product>().CountAsync(specCount);
            return _paginationHelper.Paginate<Product, ProductVm>(products, total, request.PageSize, request.PageIndex);

        }
    }
}
