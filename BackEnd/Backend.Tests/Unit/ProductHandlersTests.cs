using AutoMapper;
using Backend.Application.Contracts.Persistence;
using Backend.Application.Domain;
using Backend.Application.Features.Products.Commands.AddProduct;
using Backend.Application.Features.Products.Commands.BulkCreateRandomProducts;
using Backend.Application.Features.Products.Queries.GetProductsByParams;
using Backend.Application.Models.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Unit
{
    public class BulkCreateRandomProductsCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly BulkCreateRandomProductsCommandHandler _handler;

        public BulkCreateRandomProductsCommandHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new BulkCreateRandomProductsCommandHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidCount_ShouldInsertProductsSuccessfully()
        {
            // ARRANGE
            var command = new BulkCreateRandomProductsCommand { Count = 100 };

            _mockProductRepository
                .Setup(repo => repo.BulkInsertProductsAsync(
                    It.IsAny<IEnumerable<Product>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // ACT
            var result = await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            result.Message.Should().Contain("100");

            _mockProductRepository.Verify(
                repo => repo.BulkInsertProductsAsync(
                    It.Is<IEnumerable<Product>>(products => products.Count() == 100),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsException_ShouldReturnErrorResponse()
        {
            // ARRANGE
            var command = new BulkCreateRandomProductsCommand { Count = 100 };

            _mockProductRepository
                .Setup(repo => repo.BulkInsertProductsAsync(
                    It.IsAny<IEnumerable<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error en la base de datos"));

            // ACT
            var result = await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
            result.Message.Should().Contain("Error en la base de datos");
        }
    }

    public class AddProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly AddProductCommandHandler _handler;

        public AddProductCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockProductRepository = new Mock<IRepository<Product>>();

            _mockUnitOfWork
                .Setup(uow => uow.Repository<Product>())
                .Returns(_mockProductRepository.Object);

            _handler = new AddProductCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WithValidProduct_ShouldCreateSuccessfully()
        {
            // ARRANGE
            var command = new AddProductCommand
            {
                ProductName = "Dell PowerEdge R750",
                SupplierId = 1,
                CategoryId = 1,
                QuantityPerUnit = "1 unidad",
                UnitPrice = 4500.00m,
                UnitsInStock = 50,
                Discontinued = false
            };

            var mappedProduct = new Product(
                productName: command.ProductName,
                supplierId: command.SupplierId,
                categoryId: command.CategoryId,
                quantityPerUnit: command.QuantityPerUnit,
                unitPrice: command.UnitPrice,
                unitsInStock: command.UnitsInStock,
                unitsOnOrder: 0,
                reorderLevel: 0,
                discontinued: command.Discontinued
            );

            _mockMapper
                .Setup(m => m.Map<Product>(It.IsAny<Product>()))
                .Returns(mappedProduct);

            _mockProductRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(mappedProduct);

            // ACT
            var result = await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            result.Message.Should().Contain("exitosamente");

            _mockProductRepository.Verify(
                repo => repo.AddAsync(It.IsAny<Product>()),
                Times.Once);
        }
    }
}