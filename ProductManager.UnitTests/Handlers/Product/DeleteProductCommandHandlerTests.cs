using Moq;
using ProductManager.Application.Features.Products.Commands.Delete;
using ProductManager.Domain.Interfaces;

namespace ProductManager.UnitTests.Handlers.Product
{
    public class DeleteProductCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Delete_Product_And_SaveChanges_And_Return_True()
        {
            var repoMock = new Mock<IProductRepository>(MockBehavior.Strict);

            var id = Guid.NewGuid().ToString();

            repoMock
                .Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            repoMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new DeleteProductCommandHandler(repoMock.Object);

            var command = new DeleteProductCommand(id);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            repoMock.Verify(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
