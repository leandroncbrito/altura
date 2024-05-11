using Altura.Infrastructure.Interfaces;
using TrelloDotNet.Model;

namespace Altura.Infrastructure.ExternalServices
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

        public async Task<List<Card>> GetCardsInListAsync(string listId, CancellationToken cancellationToken)
        {
            return await _trelloApi.GetCardsInListAsync(listId, cancellationToken);
        }
    }
}
