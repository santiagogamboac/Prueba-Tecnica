using Backend.Application.Contracts.Persistence;
using Backend.Application.Domain;
using Backend.Application.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Products.Commands.BulkCreateRandomProducts
{
    public class BulkCreateRandomProductsCommandHandler : IRequestHandler<BulkCreateRandomProductsCommand, GeneralResponse>
    {
        private readonly IProductRepository _productRepository;

        public BulkCreateRandomProductsCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GeneralResponse> Handle(BulkCreateRandomProductsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var count = request.Count;

                var products = GenerateRandomProducts(count);
                await _productRepository.BulkInsertProductsAsync(products, cancellationToken);

                return new GeneralResponse(true, $"Se insertaron {count} productos.");
            }
            catch (Exception ex)
            {
                return new GeneralResponse(false, ex.Message);
            }
            
        }

        private IEnumerable<Product> GenerateRandomProducts(int count)
        {
            var products = new List<Product>(count);
            var random = new Random();

            int[] posiblesSuppliers = { 1, 2, 3};
            int[] posiblesCategories = { 1, 2};
           

            for (int i = 1; i <= count; i++)
            {
                var product = new Product(
         productName: $"Producto Aleatorio {i}",
         supplierId: posiblesSuppliers[random.Next(posiblesSuppliers.Length)],
         categoryId: posiblesCategories[random.Next(posiblesCategories.Length)],
         quantityPerUnit: $"{random.Next(1, 20)} unidades",
         unitPrice: (decimal)(random.Next(5, 100) + random.NextDouble()),
         unitsInStock: random.Next(1, 500),
         unitsOnOrder: random.Next(0, 200),
         reorderLevel: (short)random.Next(0, 50),
         discontinued: false
     );

                products.Add(product);
            }
            return products;
        }
    }
}
