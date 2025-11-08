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

namespace Backend.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, GeneralResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.ProductId);                
                await _unitOfWork.Repository<Product>().DeleteAsync(product);

                return new GeneralResponse(true, "Registro eliminado correctamente");
            }
            catch (Exception)
            {
                return new GeneralResponse(false, $"Error al eliminar");
            }
        }
    }
}
