using Altura.Application.Interfaces;
using Altura.Domain;
using Altura.Infrastructure.Interfaces;
using TrelloDotNet.Model;

namespace Altura.Application.Services
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
            UpdateCardFields(card, tender);

            await UpdateCardCustomFieldsAsync(card, tender, cancellationToken);
        }

        private async Task<Card> AddCardAsync(Card card, CancellationToken cancellationToken)
        {
            return await _trelloApi.AddCardAsync(card, cancellationToken);
        }


        private void UpdateCardFields(Card card, Tender tender)
        {
            card.Name = tender.Name;
            card.Description = tender.TenderName;
            card.Due = tender.Deadline;
        }

        private async Task UpdateCardCustomFieldsAsync(Card card, Tender tender, CancellationToken cancellationToken)
        {
            var customFields = await _trelloCustomFields.GetCustomFieldsAsync(card.BoardId, cancellationToken);

            var tenderIdField = _trelloCustomFields.GetCustomFieldByName(customFields, "TenderId");
            var lotNumberField = _trelloCustomFields.GetCustomFieldByName(customFields, "LotNumber");
            var currencyField = _trelloCustomFields.GetCustomFieldByName(customFields, "Currency");

            var updateTasks = new List<Task>
            {
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, tenderIdField, tender.TenderId.ToString(), cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, lotNumberField, tender.LotNumber, cancellationToken),
                _trelloCustomFields.UpdateCustomFieldValueAsync(card.Id, currencyField, tender.Currency, cancellationToken)
            };

            await Task.WhenAll(updateTasks);
        }
    }
}
