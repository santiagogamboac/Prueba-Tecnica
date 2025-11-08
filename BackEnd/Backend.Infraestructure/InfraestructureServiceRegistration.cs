using Backend.Application.Contracts.Persistence;
using Backend.infraestructure.Repositories;
using Backend.Infraestructure.Persistence;
using Backend.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.infraestructure
{
    public static class InfraestructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //DbContext
            services.AddDbContext<ShopDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));

            //Generic repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //services.AddHttpContextAccessor();
       
            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();


            //WebHooks


            //Service configurations


            return services;
        }
    }
}