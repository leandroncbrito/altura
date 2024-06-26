﻿using Altura.Infrastructure.Apis.Configurations;
using Altura.Infrastructure.Apis.Models;
using Altura.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
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

        public TrelloApi(IOptions<TrelloConfiguration> trelloConfig)
        {
            ApiKey = trelloConfig.Value.ApiKey;
            Token = trelloConfig.Value.Token;

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

        public async Task<Card> UpdateCardAsync(Card cardWithChanges, CancellationToken cancellationToken = default)
        {
            return await _trelloClient.UpdateCardAsync(cardWithChanges, cancellationToken);
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
