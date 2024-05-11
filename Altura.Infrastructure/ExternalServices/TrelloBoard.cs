using Altura.Infrastructure.Apis.Models;
using Altura.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using TrelloDotNet.Model;

namespace Altura.Infrastructure.ExternalServices
{
    public class TrelloBoard : ITrelloBoard
    {
        private readonly ITrelloApi _trelloApi;
        private readonly IOptions<TrelloConfiguration> _trelloConfig;

        public TrelloBoard(ITrelloApi trelloApi, IOptions<TrelloConfiguration> trelloConfig)
        {
            _trelloApi = trelloApi;
            _trelloConfig = trelloConfig;
        }

        public string GetBoardId()
        {
            return _trelloConfig.Value.BoardId;
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
