using Altura.Application.Interfaces;
using Altura.Infrastructure.Interfaces;
using TrelloDotNet.Model;

namespace Altura.Application.Services
{
    public class TrelloBoard : ITrelloBoard
    {
        private readonly ITrelloApi _trelloApi;
        public TrelloBoard(ITrelloApi trelloApi)
        {
            _trelloApi = trelloApi;
        }

        public async Task<Board> ObtainBoardAsync(string boardId, CancellationToken cancellationToken)
        {
            var existingBoard = await _trelloApi.GetBoardAsync(boardId, cancellationToken);

            if (existingBoard == null)
            {
                existingBoard = await _trelloApi.AddBoardAsync(new Board("Altura"), cancellationToken: cancellationToken);
            }

            return existingBoard;
        }

        public Task<List<List>> GetListsOnBoardAsync(string boardId, CancellationToken cancellationToken)
        {
            return _trelloApi.GetListsOnBoardAsync(boardId, cancellationToken);
        }
    }
}
