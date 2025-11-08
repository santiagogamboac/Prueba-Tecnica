using AutoMapper;
using Backend.Application.Contracts.Persistence;
using Backend.Application.Domain;
using Backend.Application.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Categories.Commands
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, GeneralResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = new Category(
                  Categoryname: request.CategoryName.Trim(),
                  description: request.Description?.Trim(),
                  picture: request.Picture?.Trim()
              );            
                await _unitOfWork.Repository<Category>().AddAsync(category);         
                return new GeneralResponse(true, $"Se ha creado la nueva categoría exitosamente");
            }
            catch (Exception ex)
            {
                return new GeneralResponse(false, ex.Message);                
            }
        }
    }
}
