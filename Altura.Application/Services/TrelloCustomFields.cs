namespace Altura
{
    public class TrelloCustomFields
    {
        private readonly TrelloApi _trelloApi;
        public TrelloCustomFields()
        {
            _trelloApi = new TrelloApi();
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

            var apiCustomFields = await _trelloApi.Client.GetCustomFieldsOnBoardAsync(boardId, cancellationToken);

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
