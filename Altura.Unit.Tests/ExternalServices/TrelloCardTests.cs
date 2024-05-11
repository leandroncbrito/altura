using Altura.Domain.Entities;
using Altura.Infrastructure.Apis.Configurations;
using Altura.Infrastructure.ExternalServices;
using Altura.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using TrelloDotNet.Model;

namespace Altura.Unit.Tests.ExternalServices
{
    public class TrelloCardTests
    {
        [Fact]
        public async Task TrelloCard_Should_Create_Card()
        {
            var mockApi = new Mock<ITrelloApi>();
            mockApi.Setup(x => x.AddCardAsync(It.IsAny<Card>(), CancellationToken.None))
                   .ReturnsAsync(new Card("card name", "test", "description"));

            var mockCustomFields = new Mock<ITrelloCustomFields>();
            
            var sut = new TrelloCard(mockApi.Object, mockCustomFields.Object);

            var id = Guid.NewGuid();
            var tenderId = Guid.NewGuid();

            var tender = new Tender
            {
                Id = id,
                Name = "name",
                TenderId = tenderId,
                TenderName = "tender name",
            };

            var response = await sut.CreateCardAsync("test", tender, CancellationToken.None);

            Assert.Equal("name", response.Name);
            Assert.Equal("tender name", response.Description);
            Assert.Equal("test", response.ListId);
        }
    }
}