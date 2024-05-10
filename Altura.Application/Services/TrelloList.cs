using Altura.Application.Interfaces;
using Altura.Infrastructure.Interfaces;
using TrelloDotNet.Model;

namespace Altura.Application.Services
{
    public class TrelloList : ITrelloList
    {
        private readonly ITrelloApi _trelloApi;

        public TrelloList(ITrelloApi trelloApi)
        {
            _trelloApi = trelloApi;
        }

        public async Task<List> AddListAsync(List list, CancellationToken cancellationToken)
        {
            return await _trelloApi.AddListAsync(list, cancellationToken);
        }
    }
}
