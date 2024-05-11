using TrelloDotNet.Model;

namespace Altura.Infrastructure.Interfaces
{
    public interface ITrelloCustomFields
    {
        Task InitializeCustomFieldsOnABoard(string boardId, CancellationToken cancellationToken);
        Task<List<CustomField>> GetCustomFieldsAsync(string boardId, CancellationToken cancellationToken);
        Task UpdateCustomFieldValueAsync(string cardId, CustomField? customField, string value, CancellationToken cancellationToken);
        CustomField? GetCustomFieldByName(IEnumerable<CustomField> customFields, string fieldName);
    }
}