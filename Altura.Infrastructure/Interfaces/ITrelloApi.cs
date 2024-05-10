using Altura.Domain;
using TrelloDotNet.Model;

namespace Altura.Infrastructure.Interfaces
{
    public interface ITrelloApi
    {
        Task<Board> GetBoardAsync(string boardId, CancellationToken cancellationToken = default(CancellationToken));
        Task<Board> AddBoardAsync(Board board, AddBoardOptions options = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<List>> GetListsOnBoardAsync(string boardId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List> AddListAsync(List list, CancellationToken cancellationToken = default(CancellationToken));
        Task<Card> AddCardAsync(Card card, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Card>> GetCardsInListAsync(string listId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<CustomField>> GetCustomFieldsOnBoardAsync(string boardId, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateCustomFieldValueOnCardAsync(string cardId, CustomField customField, string newValue, CancellationToken cancellationToken = default(CancellationToken));
        Task ClearCustomFieldValueOnCardAsync(string cardId, CustomField customField, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CreateCustomFieldOnABoardAsync(string boardId, BoardCustomField customField, CancellationToken cancellationToken = default(CancellationToken));
    }
}
