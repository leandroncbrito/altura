using TrelloDotNet.Model;

namespace Altura.Infrastructure.Interfaces
{
    public interface ITrelloList
    {
        Task<List> AddListAsync(List list, CancellationToken cancellationToken);
        Task<List<Card>> GetCardsInListAsync(string listId, CancellationToken cancellationToken);
    }
}