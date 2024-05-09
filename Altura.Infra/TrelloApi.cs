using TrelloDotNet.Model;
using TrelloDotNet;
using System.Text.Json;
using System.Text;

namespace Altura
{
    public class TrelloApi
    {
        public readonly TrelloClient Client;

        private string ApiKey;
        private string Token;

        public TrelloApi()
        {
            // get from configuration
            ApiKey = "9d34198a87f834c848e432f508e345b4";
            Token = "ATTAdea994e3c1ee4c004f4a7f66fc3f44f4b7695d492251313ffa459710bc51f88dDD195B45";

            Client = new TrelloClient(ApiKey, Token, new TrelloClientOptions(includeCustomFieldsInCardGetMethods: true));
        }

        public async Task<bool> CreateCustomFieldOnABoardAsync(string boardId, BoardCustomField customField)
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

                var response = await httpClient.PostAsync(requestUri, requestContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
