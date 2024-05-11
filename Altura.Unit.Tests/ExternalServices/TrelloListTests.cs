using Altura.Infrastructure.Apis.Configurations;
using Altura.Infrastructure.ExternalServices;
using Altura.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using TrelloDotNet.Model;

namespace Altura.Unit.Tests.ExternalServices
{
    public class TrelloListTests
    {
        [Fact]
        public async Task TrelloList_Should_Create_List()
        {
            var mockApi = new Mock<ITrelloApi>();
            mockApi.Setup(x => x.AddListAsync(It.IsAny<List>(), CancellationToken.None))
                   .ReturnsAsync(new List("List name", "test"));

            var sut = new TrelloList(mockApi.Object);

            var response = await sut.AddListAsync(new List("List name", "test"), CancellationToken.None);

            Assert.Equal("List name", response.Name);
            Assert.Equal("test", response.BoardId);
        }

        [Fact]
        public async Task TrelloBoard_Should_Return_Lists()
        {
            var mockApi = new Mock<ITrelloApi>();
            mockApi.Setup(x => x.GetCardsInListAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new List<Card> {
                    new Card("test", "Card name"),
                    new Card("test", "Card name 2")
                });

            var sut = new TrelloList(mockApi.Object);

            var response = await sut.GetCardsInListAsync("test", CancellationToken.None);

            var firstCard = response.First();

            Assert.Equal(2, response.Count);
            Assert.Equal("Card name", firstCard.Name);
            Assert.Equal("test", firstCard.ListId);
        }
    }
}