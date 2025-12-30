using Moq;
using ProductManager.Application.Features.Products.Commands.Create; 
using ProductManager.Domain.Interfaces;

namespace ProductManager.UnitTests.Handlers.Product
{
    public class CreateProductCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Add_Product_And_SaveChanges_And_Return_Id()
        {
            var repoMock = new Mock<IProductRepository>(MockBehavior.Strict);

            Domain.Entities.Product? capturedProduct = null;

            repoMock
                .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
                .Callback<Domain.Entities.Product, CancellationToken>((p, ct) => capturedProduct = p)
                .Returns(Task.CompletedTask);

            repoMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new CreateProductCommandHandler(repoMock.Object);

            var command = new CreateProductCommand(
                Name: "Test product",
                Description: "Desc",
                Price: 12.5m,
                Quantity: 3,
                Active: true);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.True(Guid.TryParse(result, out _));

            repoMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Product>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(capturedProduct);
            Assert.Equal(command.Name, capturedProduct!.Name);
            Assert.Equal(command.Description, capturedProduct.Description);
            Assert.Equal(command.Price, capturedProduct.Price);
            Assert.Equal(command.Quantity, capturedProduct.StockQuantity);
            Assert.Equal(command.Active, capturedProduct.IsActive);
        }
    }
}
