using Altura.Infrastructure.Apis.Configurations;
using Altura.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using TrelloDotNet.Model;

namespace Altura.Unit.Tests.ExternalServices
{

    public abstract class TrelloBase
    {
        protected Mock<IOptions<TrelloConfiguration>> MockConfig;
        protected TrelloBase()
        {
            MockConfig = new Mock<IOptions<TrelloConfiguration>>();

            MockConfig.Setup(x => x.Value)
                .Returns(new TrelloConfiguration
                {
                    ApiKey = "test",
                    Token = "test",
                    BoardId = "test"
                });
        }
    }
}
