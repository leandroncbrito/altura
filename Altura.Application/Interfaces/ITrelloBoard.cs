using TrelloDotNet.Model;

namespace Altura.Application.Interfaces
{
    public interface ITrelloBoard
    {
        string GetBoardId();
        Task<Board> ObtainBoardAsync(string boardId, CancellationToken cancellationToken);

        Task<List<List>> GetListsOnBoardAsync(string boardId, CancellationToken cancellationToken);
    }
}
