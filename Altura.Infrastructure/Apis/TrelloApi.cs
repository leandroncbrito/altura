using Altura.Domain;
using Altura.Infrastructure.Interfaces;
using System.Text;
using System.Text.Json;
using TrelloDotNet;
using TrelloDotNet.Model;

namespace Altura.Infrastructure.Apis
{
    public class TrelloApi : ITrelloApi
    {
        public readonly TrelloClient _trelloClient;

        private string ApiKey;
        private string Token;

        public TrelloApi()
        {
            // get from configuration
            ApiKey = "9d34198a87f834c848e432f508e345b4";
            Token = "ATTAdea994e3c1ee4c004f4a7f66fc3f44f4b7695d492251313ffa459710bc51f88dDD195B45";

            _trelloClient = new TrelloClient(ApiKey, Token, new TrelloClientOptions(includeCustomFieldsInCardGetMethods: true));
        }

        public async Task<Board> AddBoardAsync(Board board, AddBoardOptions options = null, CancellationToken cancellationToken = default)
        {
            return await _trelloClient.AddBoardAsync(board, options, cancellationToken);
        }

        public async Task<Card> AddCardAsync(Card card, CancellationToken cancellationToken = default)
        {
            return await _trelloClient.AddCardAsync(card, cancellationToken);
        }

        public async Task<Board> GetBoardAsync(string boardId, CancellationToken cancellationToken = default)
        {
            return await _trelloClient.GetBoardAsync(boardId, cancellationToken);
        }

        public async Task<List<List>> GetListsOnBoardAsync(string boardId, CancellationToken cancellationToken = default)
        {
            return await _trelloClient.GetListsOnBoardAsync(boardId, cancellationToken);
        }

        public async Task<List> AddListAsync(List list, CancellationToken cancellationToken = default)
        {
            return await _trelloClient.AddListAsync(list, cancellationToken);
        }

        public async Task<List<Card>> GetCardsInListAsync(string listId, CancellationToken cancellationToken = default)
        {
            return await _trelloClient.GetCardsInListAsync(listId, cancellationToken);
        }

        public async Task<List<CustomField>> GetCustomFieldsOnBoardAsync(string boardId, CancellationToken cancellationToken = default)
        {
            return await _trelloClient.GetCustomFieldsOnBoardAsync(boardId, cancellationToken);
        }

        public async Task UpdateCustomFieldValueOnCardAsync(string cardId, CustomField customField, string newValue, CancellationToken cancellationToken = default)
        {
            await _trelloClient.UpdateCustomFieldValueOnCardAsync(cardId, customField, newValue, cancellationToken);
        }

        public async Task ClearCustomFieldValueOnCardAsync(string cardId, CustomField customField, CancellationToken cancellationToken = default)
        {
            await _trelloClient.ClearCustomFieldValueOnCardAsync(cardId, customField, cancellationToken);
        }

        public async Task<bool> CreateCustomFieldOnABoardAsync(string boardId, BoardCustomField customField, CancellationToken cancellationToken = default)
        {
            try
            {
                using var httpClient = new HttpClient();

                var requestUri = $"https://api.trello.com/1/customFields?key={ApiKey}&token={Token}";

                var requestData = new
                {
                    idModel = boardId,
                    modelType = customField.ModelType,
                    name = customField.Name,
                    type = customField.Type
                };

                var jsonContent = JsonSerializer.Serialize(requestData);
                var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(requestUri, requestContent, cancellationToken);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
