using Backend.Application.Models.ViewModels.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Categories.Queries
{
    public class GetCategoriesQuery : IRequest<List<CategoryVm>>
    {
    }
}
