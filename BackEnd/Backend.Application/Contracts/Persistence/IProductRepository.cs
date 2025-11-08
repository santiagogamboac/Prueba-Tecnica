using Backend.Application.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Contracts.Persistence
{
    public interface IProductRepository
    {
        Task BulkInsertProductsAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default);
    }
}
