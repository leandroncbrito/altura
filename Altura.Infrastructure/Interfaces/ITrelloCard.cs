using Altura.Domain.Entities;
using TrelloDotNet.Model;

namespace Altura.Infrastructure.Interfaces
{
    public interface ITrelloCard
    {
        Task<Card> CreateCardAsync(string listId, Tender tender, CancellationToken cancellationToken);
        Task<List<Card>> GetCardsInListAsync(string listId, CancellationToken cancellationToken);
        Task UpdateCardAsync(Card card, Tender tender, CancellationToken cancellationToken);
    }
}