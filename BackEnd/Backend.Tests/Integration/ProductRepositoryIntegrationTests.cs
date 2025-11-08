using Backend.Application.Domain;
using Backend.Infraestructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Integration
{
    public class ProductRepositoryIntegrationTests : IDisposable
    {
        private readonly ShopDbContext _context;

        public ProductRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ShopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ShopDbContext(options);
            SeedDatabase();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            var categories = new[]
            {
                new Category( "SERVIDORES", "", ""), 
                new Category ( "CLOUD",  "", "" )
            };
            _context.Categories.AddRange(categories);

            var suppliers = new[]
            {
                new Supplier { Supplierid = 1, Companyname = "Dell Inc." },
                new Supplier { Supplierid = 2, Companyname = "HP Enterprise" }
            };
            _context.Suppliers.AddRange(suppliers);

            var products = new[]
            {
                new Product("Dell PowerEdge R750", 1, 1, "1 unidad", 4500m, 50, 10, 5, false),
                new Product("HP ProLiant DL380", 2, 1, "1 unidad", 3800m, 30, 5, 10, false)
            };
            _context.Products.AddRange(products);
            _context.SaveChanges();
        }

        [Fact]
        public async Task AddProduct_ShouldInsertSuccessfully()
        {
            // ARRANGE
            var newProduct = new Product(
                "Lenovo ThinkSystem",
                1,
                1,
                "1 unidad",
                4200m,
                25,
                0,
                5,
                false
            );

            // ACT
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            // ASSERT
            var savedProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Productname == "Lenovo ThinkSystem");

            savedProduct.Should().NotBeNull();
            savedProduct.Unitprice.Should().Be(4200m);
        }

        [Fact]
        public async Task GetProductsByCategory_ShouldFilterCorrectly()
        {
            // ACT
            var products = await _context.Products
                .Where(p => p.Categoryid == 1)
                .ToListAsync();

            // ASSERT
            products.Should().HaveCount(2);
            products.All(p => p.Categoryid == 1).Should().BeTrue();
        }
    }
}