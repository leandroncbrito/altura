using Altura.Infrastructure.Apis.Configurations;
using Altura.Infrastructure.ExternalServices;
using Altura.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using TrelloDotNet.Model;

namespace Altura.Unit.Tests.ExternalServices
{
    public class TrelloBoardTests
    {
        protected Mock<IOptions<TrelloConfiguration>> _mockConfig;

        public TrelloBoardTests()
        {
            _mockConfig = new Mock<IOptions<TrelloConfiguration>>();

            _mockConfig.Setup(x => x.Value)
                .Returns(new TrelloConfiguration
                {
                    ApiKey = "test",
                    Token = "test",
                    BoardId = "test"
                });
        }

        [Fact]
        public async Task TrelloBoard_Should_Return_Board()
        {
            var mockApi = new Mock<ITrelloApi>();
            mockApi.Setup(x => x.GetBoardAsync(It.IsAny<string>(), CancellationToken.None))
                   .ReturnsAsync(new Board("Board name", "Board description"));

            var sut = new TrelloBoard(mockApi.Object, _mockConfig.Object);

            var response = await sut.ObtainBoardAsync("test", CancellationToken.None);

            Assert.Equal("Board name", response.Name);
            Assert.Equal("Board description", response.Description);
        }

        [Fact]
        public async Task TrelloBoard_Should_Return_Lists()
        {
            var mockApi = new Mock<ITrelloApi>();
            mockApi.Setup(x => x.GetListsOnBoardAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new List<List> {
                    new List("List name", "test"),
                    new List("List name 2", "test")
                });

            var sut = new TrelloBoard(mockApi.Object, _mockConfig.Object);

            var response = await sut.GetListsOnBoardAsync("test", CancellationToken.None);

            var firstList = response.First();

            Assert.Equal(2, response.Count);
            Assert.Equal("List name", firstList.Name);
        }
    }
}