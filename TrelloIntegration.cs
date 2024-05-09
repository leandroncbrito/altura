using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using TrelloDotNet;
using TrelloDotNet.Model;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

namespace Altura
{
    public class TrelloIntegration
    {        
        private readonly TrelloClient TrelloClient;

        private readonly TrelloBoard _trelloBoard;
        private readonly TrelloCustomFields _trelloCustomFields;

        private string ApiKey;
        private string Token;
        private string BoardId;

        public TrelloIntegration(TrelloBoard trelloBoard, TrelloCustomFields trelloCustomFields)
        {
            ApiKey = "9d34198a87f834c848e432f508e345b4";
            Token = "ATTAdea994e3c1ee4c004f4a7f66fc3f44f4b7695d492251313ffa459710bc51f88dDD195B45";
            BoardId = "N20YA7Aj";
                        
            TrelloClient = new TrelloClient(ApiKey, Token, new TrelloClientOptions(includeCustomFieldsInCardGetMethods: true));

            _trelloBoard = trelloBoard;
            _trelloCustomFields = trelloCustomFields;
        }

        public async Task GetCard(IEnumerable<Tender> tenders, CancellationToken cancellationToken)
        {
            //var board = await ObtainBoardAsync(cancellationToken);
            //await CreateCustomFieldsAsync(board.Id, cancellationToken);

            var board = await _trelloBoard.ObtainBoardAsync(BoardId, cancellationToken);
            await _trelloCustomFields.InitializeCustomFieldsOnABoard(board.Id, cancellationToken);

            var listsMap = new Dictionary<int, string>();

            var lists = await TrelloClient.GetListsOnBoardAsync(board.Id, cancellationToken);

            var limit = 5;
                        
            foreach (var tender in tenders)
            {
                string listId;

                var status = tender.Status;

                if (listsMap.ContainsKey(status) is false)
                {
                    var listName = $"Status:{status}";

                    var list = lists.FirstOrDefault(x => x.Name.IsEqualTo(listName));

                    list ??= await TrelloClient.AddListAsync(new List(listName, board.Id), cancellationToken);

                    listsMap.Add(status, list.Id);
                }

                listId = listsMap[status];

                var customFields = await GetCustomFieldsAsync(board.Id, cancellationToken);

                var tenderId = customFields.First(x => x.Name.IsEqualTo("TenderId"));
                var lotNumber = customFields.First(x => x.Name.IsEqualTo("LotNumber"));

                var apiCards = await TrelloClient.GetCardsInListAsync(listId, cancellationToken);

                Card? card = null;

                foreach (var apiCard in apiCards)
                {
                    var cardTenderId = apiCard.CustomFieldItems.FirstOrDefault(x => x.CustomFieldId.IsEqualTo(tenderId.Id));
                    var cardLotNumber = apiCard.CustomFieldItems.FirstOrDefault(x => x.CustomFieldId.IsEqualTo(lotNumber.Id));
                    
                    if (cardTenderId != null && cardLotNumber != null)
                    {
                        if (tender.TenderId.ToString().IsEqualTo(cardTenderId.Value.TextAsString) &&
                            tender.LotNumber.IsEqualTo(cardLotNumber.Value.TextAsString))
                        {
                            card = apiCard;

                            await UpdateCardAsync(card, tender, cancellationToken);

                            break;
                        }
                    }
                }

                if (card == null)
                {
                    await CreateCardAsync(listId, tender, cancellationToken);
                }


                limit--;
                if (limit == 0)
                {
                    break;
                }
            }
        }

        private async Task<Card> CreateCardAsync(string listId, Tender tender, CancellationToken cancellationToken)
        {
            var card = InitializeCard(listId, tender);

            var cardCreated = await AddCardAsync(card, cancellationToken);

            await UpdateCardCustomFieldsAsync(cardCreated, tender, cancellationToken);

            return card;
        }

        private Card InitializeCard(string listId, Tender tender)
        {
            var card = new Card(listId, tender.Name, tender.TenderName);
            card.Due = tender.Deadline;

            return card;
        }

        private async Task<Card> AddCardAsync(Card card, CancellationToken cancellationToken)
        {
            return await TrelloClient.AddCardAsync(card, cancellationToken);
        }


        private async Task UpdateCardAsync(Card card, Tender tender, CancellationToken cancellationToken)
        {
            UpdateCardFields(card, tender);

            await UpdateCardCustomFieldsAsync(card, tender, cancellationToken);
        }


        private void UpdateCardFields(Card card, Tender tender)
        {
            card.Name = tender.Name;
            card.Description = tender.TenderName;
            card.Due = tender.Deadline;
        }

        private async Task UpdateCardCustomFieldsAsync(Card card, Tender tender, CancellationToken cancellationToken)
        {
            var customFields = await GetCustomFieldsAsync(card.BoardId, cancellationToken);

            var tenderIdField = GetCustomFieldByName(customFields, "TenderId");
            var lotNumberField = GetCustomFieldByName(customFields, "LotNumber");
            var currencyField = GetCustomFieldByName(customFields, "Currency");

            var updateTasks = new List<Task>
            {
                UpdateCustomFieldValueAsync(card.Id, tenderIdField, tender.TenderId.ToString(), cancellationToken),
                UpdateCustomFieldValueAsync(card.Id, lotNumberField, tender.LotNumber, cancellationToken),
                UpdateCustomFieldValueAsync(card.Id, currencyField, tender.Currency, cancellationToken)
            };

            await Task.WhenAll(updateTasks);
        }

        private async Task<List<CustomField>> GetCustomFieldsAsync(string boardId, CancellationToken cancellationToken)
        {
            return await TrelloClient.GetCustomFieldsOnBoardAsync(boardId, cancellationToken);
        }

        private CustomField? GetCustomFieldByName(IEnumerable<CustomField> customFields, string fieldName)
        {
            return customFields.FirstOrDefault(field => field.Name.IsEqualTo(fieldName));
        }

        private async Task UpdateCustomFieldValueAsync(string cardId, CustomField? customField, string value, CancellationToken cancellationToken)
        {
            if (customField == null)
            {
                return;
            }

            await TrelloClient.UpdateCustomFieldValueOnCardAsync(cardId, customField, value, cancellationToken);
        }

        private async Task<Board> ObtainBoardAsync(CancellationToken cancellationToken)
        {
            var existingBoard = await TrelloClient.GetBoardAsync(BoardId, cancellationToken);

            if (existingBoard == null)
            {
                existingBoard = await TrelloClient.AddBoardAsync(new Board("Altura"), cancellationToken: cancellationToken);
            }

            return existingBoard;
        }

        private async Task CreateCustomFieldsAsync(string boardId, CancellationToken cancellationToken)
        {
            var customFieldsToCreate = new List<BoardCustomField>
            {
                new BoardCustomField("Id", BoardCustomFieldType.Text),
                new BoardCustomField("TenderId", BoardCustomFieldType.Text),
                new BoardCustomField("LotNumber", BoardCustomFieldType.Text),
                new BoardCustomField("ExpirationDate", BoardCustomFieldType.Date),
                new BoardCustomField("HasDocuments", BoardCustomFieldType.Checkbox),
                new BoardCustomField("Location", BoardCustomFieldType.Text),
                new BoardCustomField("PublicationDate", BoardCustomFieldType.Date),
                new BoardCustomField("Status", BoardCustomFieldType.Number),
                new BoardCustomField("Currency", BoardCustomFieldType.Text),
                new BoardCustomField("Value", BoardCustomFieldType.Number),
            };

            var createCustomFieldOnABoardTasks = new List<Task>();

            var apiCustomFields = await TrelloClient.GetCustomFieldsOnBoardAsync(boardId, cancellationToken);

            foreach (var customField in customFieldsToCreate)
            {                                
                if (apiCustomFields.Exists(x => x.Name.IsEqualTo(customField.Name)) is false)
                {
                    createCustomFieldOnABoardTasks.Add(CreateCustomFieldOnABoardAsync(boardId, customField));                    
                }
            }

            await Task.WhenAll(createCustomFieldOnABoardTasks);
        }

        private async Task<bool> CreateCustomFieldOnABoardAsync(string boardId, BoardCustomField customField)
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

        public IEnumerable<Tender> GetTenders()
        {
            var models = new List<Tender>();
            
            using (var reader = new StreamReader("assignment-opportunities-v1.csv"))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                while (csv.Read())
                {
                    csv.Context.RegisterClassMap<TenderMap>();

                    models.Add(csv.GetRecord<Tender>());
                }
            }
            
            return models;
        }
    }
}
