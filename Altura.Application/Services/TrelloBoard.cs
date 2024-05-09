using Altura.Application;
using Altura.Application.Interfaces;
using TrelloDotNet.Model;

namespace Altura
{
    public class TrelloBoard : ITrelloBoard
    {
        private readonly TrelloApi _trelloApi;
        public TrelloBoard() 
        { 
            _trelloApi = new TrelloApi();
        }

        public async Task<Board> ObtainBoardAsync(string boardId, CancellationToken cancellationToken)
        {
            var existingBoard = await _trelloApi.Client.GetBoardAsync(boardId, cancellationToken);

            if (existingBoard == null)
            {
                existingBoard = await _trelloApi.Client.AddBoardAsync(new Board("Altura"), cancellationToken: cancellationToken);
            }

            return existingBoard;
        }
    }
}
