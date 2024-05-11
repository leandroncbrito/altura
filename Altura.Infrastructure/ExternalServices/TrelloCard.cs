using Altura.Domain.Entities;
using Altura.Infrastructure.Apis.Models;
using Altura.Infrastructure.Interfaces;
using Altura.Infrastructure.Transformations;
using TrelloDotNet.Model;

namespace Altura.Infrastructure.ExternalServices
{
    public class TrelloCard : ITrelloCard
    {
        private readonly ITrelloApi _trelloApi;
        private readonly ITrelloCustomFields _trelloCustomFields;

        public TrelloCard(ITrelloApi trelloApi, ITrelloCustomFields trelloCustomFields)
        {
            _trelloApi = trelloApi;
            _trelloCustomFields = trelloCustomFields;
        }

        public async Task<Card> CreateCardAsync(string listId, Tender tender, CancellationToken cancellationToken)
        {
            var card = new Card(listId, tender.Name, tender.TenderName);
            card.Due = tender.Deadline;

            var cardCreated = await AddCardAsync(card, cancellationToken);

            await UpdateCardCustomFieldsAsync(cardCreated, tender, cancellationToken);

            return card;
        }

        public async Task UpdateCardAsync(Card card, Tender tender, CancellationToken cancellationToken)
        {
            await UpdateCardFieldsAsync(card, tender, cancellationToken);

            await UpdateCardCustomFieldsAsync(card, tender, cancellationToken);
        }

        private async Task<Card> AddCardAsync(Card card, CancellationToken cancellationToken)
        {
            return await _trelloApi.AddCardAsync(card, cancellationToken);
        }

        private async Task UpdateCardFieldsAsync(Card card, Tender tender, CancellationToken cancellationToken)
        {
            card.Name = tender.Name;
            card.Description = tender.TenderName;
            card.Due = tender.Deadline;

            await _trelloApi.UpdateCardAsync(card, cancellationToken);
        }

        private async Task UpdateCardCustomFieldsAsync(Card card, Tender tender, CancellationToken cancellationToken)
        {
            var customFields = await _trelloCustomFields.GetCustomFieldsAsync(card.BoardId, cancellationToken);

            var idField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.Id.ToString());
            var tenderIdField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.TenderId.ToString());
            var lotNumberField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.LotNumber.ToString());
            var expirationDateField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.ExpirationDate.ToString());
            var hasDocumentsField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.HasDocuments.ToString());
            var locationField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.Location.ToString());
            var publicationDateField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.PublicationDate.ToString());
            var statusField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.Status.ToString());
            var currencyField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.Currency.ToString());
            var valueField = _trelloCustomFields.GetCustomFieldByName(customFields, BoardCustomFieldName.Value.ToString());

            var updateTasks = new List<Task>
            {
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, idField, idField.ConvertToString(tender.Id), cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, tenderIdField, tenderIdField.ConvertToString(tender.TenderId), cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, lotNumberField, tender.LotNumber, cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, expirationDateField, expirationDateField.ConvertToString(tender.ExpirationDate), cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, hasDocumentsField, hasDocumentsField.ConvertToString(tender.HasDocuments), cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, locationField, tender.Location, cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, publicationDateField, publicationDateField.ConvertToString(tender.PublicationDate), cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, statusField, statusField.ConvertToString(tender.Status), cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, currencyField, tender.Currency, cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, valueField, valueField.ConvertToString(tender.Value), cancellationToken),
            };

            await Task.WhenAll(updateTasks);
        }
    }
}
