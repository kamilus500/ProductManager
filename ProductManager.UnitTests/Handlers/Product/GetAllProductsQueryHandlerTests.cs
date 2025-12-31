using Moq;
using ProductManager.Application.Features.Products.Queries.GetAll;
using ProductManager.Domain.Interfaces;

namespace ProductManager.UnitTests.Handlers.Product
{
    public class GetAllProductsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_PageResult_With_Items_And_TotalCount()
        {
            var repoMock = new Mock<IProductRepository>();

            var products = new List<Domain.Entities.Product>
            {
                new Domain.Entities.Product("A", 1.1m, 2, true, "d1"),
                new Domain.Entities.Product("B", 2.2m, 5, false, "d2")
            };

            repoMock
               .Setup(r => r.GetAllAsync(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.IsAny<string?>(),
                   It.IsAny<string?>(),
                   It.IsAny<decimal?>(),
                   It.IsAny<decimal?>(),
                   It.IsAny<bool?>(),
                   It.IsAny<bool?>(),
                   It.IsAny<decimal?>(),
                   It.IsAny<decimal?>(),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(((IEnumerable<Domain.Entities.Product>)products, products.Count));

            var handler = new GetAllProductsQueryHandler(repoMock.Object);

            var query = new GetAllProductsQuery(PageNumber: 1, PageSize: 10);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(query.PageNumber, result.PageNumber);
            Assert.Equal(query.PageSize, result.PageSize);
            Assert.Equal(products.Count, result.Items.Count());
            Assert.Equal(products.Count, result.TotalCount);
        }
    }
}
