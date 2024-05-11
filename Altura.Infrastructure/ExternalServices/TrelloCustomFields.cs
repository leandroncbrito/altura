using Altura.Infrastructure.Apis.Models;
using Altura.Infrastructure.Helpers;
using Altura.Infrastructure.Interfaces;
using TrelloDotNet.Model;

namespace Altura.Infrastructure.ExternalServices
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

        public async Task UpdateCustomFieldValueAsync(string cardId, CustomField customField, string value, CancellationToken cancellationToken)
        {
            if (customField == null)
            {
                return;
            }

            if (value == null)
            {
                await _trelloApi.ClearCustomFieldValueOnCardAsync(cardId, customField, cancellationToken);
                return;
            }

            await _trelloApi.UpdateCustomFieldValueOnCardAsync(cardId, customField, value, cancellationToken);
        }

        public async Task InitializeCustomFieldsOnABoard(string boardId, CancellationToken cancellationToken)
        {
            var customFieldsToCreate = new List<BoardCustomField>
            {
                new BoardCustomField(BoardCustomFieldName.Id, BoardCustomFieldType.Text),
                new BoardCustomField(BoardCustomFieldName.TenderId, BoardCustomFieldType.Text),
                new BoardCustomField(BoardCustomFieldName.LotNumber, BoardCustomFieldType.Text),
                new BoardCustomField(BoardCustomFieldName.ExpirationDate, BoardCustomFieldType.Date),
                new BoardCustomField(BoardCustomFieldName.HasDocuments, BoardCustomFieldType.Checkbox),
                new BoardCustomField(BoardCustomFieldName.Location, BoardCustomFieldType.Text),
                new BoardCustomField(BoardCustomFieldName.PublicationDate, BoardCustomFieldType.Date),
                new BoardCustomField(BoardCustomFieldName.Status, BoardCustomFieldType.Number),
                new BoardCustomField(BoardCustomFieldName.Currency, BoardCustomFieldType.Text),
                new BoardCustomField(BoardCustomFieldName.Value, BoardCustomFieldType.Number),
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
