using Backend.Application.Contracts.Persistence;
using Backend.Application.Domain;
using Backend.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infraestructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopDbContext _context;

        public ProductRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task BulkInsertProductsAsync(IEnumerable<Product> products,CancellationToken cancellationToken = default)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                var npgsqlConnection = connection as NpgsqlConnection;

                if (npgsqlConnection == null)
                {
                    throw new InvalidOperationException("La conexión de EF Core no es de Npgsql.");
                }
         
                if (npgsqlConnection.State != ConnectionState.Open)
                {
                    await npgsqlConnection.OpenAsync(cancellationToken);
                }

                string copyCommand = "COPY products (" +"productname, supplierid, categoryid, quantityperunit, unitprice, unitsinstock, unitsonorder, reorderlevel, discontinued" +          
                                     ") FROM STDIN (FORMAT BINARY)";

                using (var writer = npgsqlConnection.BeginBinaryImport(copyCommand))
                {
                    foreach (var product in products)
                    {                       
                        await writer.StartRowAsync(cancellationToken);                                   
                        await writer.WriteAsync(product.Productname, cancellationToken);
                        await writer.WriteAsync(product.Supplierid, cancellationToken);
                        await writer.WriteAsync(product.Categoryid, cancellationToken);
                        await writer.WriteAsync(product.Quantityperunit, cancellationToken);
                        await writer.WriteAsync(product.Unitprice, cancellationToken);
                        await writer.WriteAsync(product.Unitsinstock, cancellationToken);
                        await writer.WriteAsync(product.Unitsonorder, cancellationToken);
                        await writer.WriteAsync(product.Reorderlevel, cancellationToken);
                        await writer.WriteAsync(product.Discontinued, cancellationToken);
                    }
                    
                    await writer.CompleteAsync(cancellationToken);
                    await writer.CloseAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
    }
}
