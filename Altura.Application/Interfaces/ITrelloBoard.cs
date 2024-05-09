using TrelloDotNet.Model;

namespace Altura.Application.Interfaces
{
    public interface ITrelloBoard
    {
        Task<Board> ObtainBoardAsync(string boardId, CancellationToken cancellationToken);
    }
}
