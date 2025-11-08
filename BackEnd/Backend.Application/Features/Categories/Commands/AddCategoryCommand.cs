using Backend.Application.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Categories.Commands
{
    public class AddCategoryCommand : IRequest<GeneralResponse>
    {
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
    }
}
