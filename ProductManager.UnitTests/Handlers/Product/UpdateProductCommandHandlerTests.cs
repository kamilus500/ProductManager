using Moq;
using ProductManager.Application.Features.Products.Commands.Update;
using ProductManager.Domain.Interfaces;
using System.Reflection;

namespace ProductManager.UnitTests.Handlers.Product
{
    public class UpdateProductCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Update_Product_And_SaveChanges_And_Return_UpdatedDto()
        {
            var repoMock = new Mock<IProductRepository>(MockBehavior.Strict);

            var existingProduct = new Domain.Entities.Product("OldName", 10.0m, 5, false, "olddesc");
            var id = Guid.NewGuid();

            var idProp = typeof(Domain.Entities.Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!;
            idProp.SetValue(existingProduct, id);

            repoMock
                .Setup(r => r.GetById(id.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            repoMock
                .Setup(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            repoMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new UpdateProductCommandHandler(repoMock.Object);

            var command = new UpdateProductCommand(
                Id: id.ToString(),
                Name: "NewName",
                Price: 20.0m,
                Quantity: 8,
                Description: "newdesc",
                IsActive: true);


            var result = await handler.Handle(command, CancellationToken.None);

            repoMock.Verify(r => r.GetById(id.ToString(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
            repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(id.ToString(), result.Id);
            Assert.Equal(command.Name, result.Name);

            Assert.Equal(existingProduct.Price, result.Price);
            Assert.Equal(command.Quantity, result.StockQuantity);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.IsActive, result.IsActive);
        }
    }
}
