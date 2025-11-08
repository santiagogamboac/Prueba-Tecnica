using AutoMapper;
using Backend.Application.Contracts.Persistence;
using Backend.Application.Domain;
using Backend.Application.Models.ViewModels.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Categories.Queries
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<CategoryVm>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {

            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            return _mapper.Map<List<CategoryVm>>(categories);
        }

    }
}
