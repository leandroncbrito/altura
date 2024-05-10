using Altura.Application.Interfaces;
using Altura.Domain;
using Altura.Infrastructure.Helpers;
using Altura.Infrastructure.Interfaces;
using TrelloDotNet.Model;

namespace Altura.Application.Services
{
    public class TrelloCustomFields : ITrelloCustomFields
    {
        private readonly ITrelloApi _trelloApi;
        public TrelloCustomFields(ITrelloApi trelloApi)
        {
            _trelloApi = trelloApi;
        }

        public async Task<List<CustomField>> GetCustomFieldsAsync(string boardId, CancellationToken cancellationToken)
        {
            return await _trelloApi.GetCustomFieldsOnBoardAsync(boardId, cancellationToken);
        }

        public CustomField? GetCustomFieldByName(IEnumerable<CustomField> customFields, string fieldName)
        {
            return customFields.FirstOrDefault(field => field.Name.IsEqualTo(fieldName));
        }

        public async Task UpdateCustomFieldValueAsync(string cardId, CustomField? customField, string value, CancellationToken cancellationToken)
        {
            if (customField == null)
            {
                return;
            }

            await _trelloApi.UpdateCustomFieldValueOnCardAsync(cardId, customField, value, cancellationToken);
        }

        public async Task InitializeCustomFieldsOnABoard(string boardId, CancellationToken cancellationToken)
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

            var apiCustomFields = await _trelloApi.GetCustomFieldsOnBoardAsync(boardId, cancellationToken);

            foreach (var customField in customFieldsToCreate)
            {
                if (apiCustomFields.Exists(x => x.Name.IsEqualTo(customField.Name)) is false)
                {
                    createCustomFieldOnABoardTasks.Add(_trelloApi.CreateCustomFieldOnABoardAsync(boardId, customField));
                }
            }

            await Task.WhenAll(createCustomFieldOnABoardTasks);
        }
    }
}
