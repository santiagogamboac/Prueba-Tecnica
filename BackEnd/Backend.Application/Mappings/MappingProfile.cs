using AutoMapper;
using Backend.Application.Domain;
using Backend.Application.Models.ViewModels.Access;
using Backend.Application.Models.ViewModels.Categories;
using Backend.Application.Models.ViewModels.Products;


namespace Backend.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var @types = new Dictionary<Type, Type>
            {
                   
                {typeof(Product), typeof(ProductVm) },
                {typeof(Category), typeof(CategoryVm) },

            };

            #region Queries
            RegisterQueryMapings(@types);
            /*CreateMap<ProductClient, ProductCustomerVm>()
                        .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Client.Name))
                        .ForMember(d => d.CustomerIdentification, o => o.MapFrom(s => s.Client.Identification))
                        .ForMember(d => d.CustomerCompany, o => o.MapFrom(s => s.Client.Company.Name))
                        .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                        .ForMember(d => d.Value, o => o.MapFrom(s => s.Product.Value));*/
            CreateMap<Product, ProductVm>()
                       .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Categoryname))
                       .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.Category.Picture))
                       .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Categoryid));
            #endregion

            #region Commands
            RegisterCommandMapings(@types);
            #endregion
        }

        private void RegisterQueryMapings(Dictionary<Type, Type> @types)
        {
            foreach (var @type in @types)
                CreateMap(@type.Key, type.Value);
        }

        private void RegisterCommandMapings(Dictionary<Type, Type> @types)
        {
            foreach (var @type in @types)
                CreateMap(@type.Value, type.Key);
        }
    }
}
