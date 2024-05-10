using Altura.Application.Interfaces;
using Altura.Domain;
using Altura.Infrastructure.Apis.Models;
using Altura.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;
using TrelloDotNet.Model;

namespace Altura.Application.Services
{
    public class TrelloIntegration : ITrelloIntegration
    {
        private readonly ITrelloBoard _trelloBoard;
        private readonly ITrelloList _trelloList;
        private readonly ITrelloCard _trelloCard;
        private readonly ITrelloCustomFields _trelloCustomFields;
        private readonly ILogger<TrelloIntegration> _logger;

        public TrelloIntegration(
            ITrelloBoard trelloBoard,
            ITrelloList trelloList,
            ITrelloCard trelloCard,
            ITrelloCustomFields trelloCustomFields,
            ILogger<TrelloIntegration> logger)
        {
            _trelloBoard = trelloBoard;
            _trelloList = trelloList;
            _trelloCustomFields = trelloCustomFields;
            _trelloCard = trelloCard;
            _logger = logger;
        }

        public async Task TransformTendersToCards(IEnumerable<Tender> tenders, CancellationToken cancellationToken)
        {
            try
            {
                var boardId = _trelloBoard.GetBoardId();

                var board = await _trelloBoard.ObtainBoardAsync(boardId, cancellationToken);

                await _trelloCustomFields.InitializeCustomFieldsOnABoard(board.Id, cancellationToken);

                var lists = await _trelloBoard.GetListsOnBoardAsync(board.Id, cancellationToken);

                var listsMap = new Dictionary<int, string>();

                var customFields = await _trelloCustomFields.GetCustomFieldsAsync(board.Id, cancellationToken);

                foreach (var tender in tenders)
                {
                    var listId = await GetListIdAsync(board.Id, lists, listsMap, tender.Status, cancellationToken);

                    var card = await FindCardAsync(listId, customFields, tender, cancellationToken);

                    if (card == null)
                    {
                        await _trelloCard.CreateCardAsync(listId, tender, cancellationToken);
                    }
                    else
                    {
                        await _trelloCard.UpdateCardAsync(card, tender, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error transforming tenders to cards: " + ex.Message, ex);

                throw;
            }
        }

        private async Task<string> GetListIdAsync(string boardId, List<List> lists, Dictionary<int, string> listsMap, int status, CancellationToken cancellationToken)
        {
            string listId;

            if (listsMap.ContainsKey(status) is false)
            {
                var listName = $"Status:{status}";

                var list = lists.FirstOrDefault(x => x.Name.IsEqualTo(listName));

                list ??= await _trelloList.AddListAsync(new List(listName, boardId), cancellationToken);

                listsMap.Add(status, list.Id);
            }

            listId = listsMap[status];

            return listId;
        }

        private async Task<Card?> FindCardAsync(string listId, IEnumerable<CustomField> customFields, Tender tender, CancellationToken cancellationToken)
        {
            var apiCards = await _trelloCard.GetCardsInListAsync(listId, cancellationToken);

            foreach (var apiCard in apiCards)
            {
                if (CardMatchesTender(apiCard, customFields, tender))
                {
                    return apiCard;
                }
            }
            return null;
        }

        private bool CardMatchesTender(Card apiCard, IEnumerable<CustomField> customFields, Tender tender)
        {
            var tenderId = customFields.First(x => x.Name.IsEqualTo(BoardCustomFieldName.TenderId.ToString()));
            var lotNumber = customFields.First(x => x.Name.IsEqualTo(BoardCustomFieldName.LotNumber.ToString()));

            var cardTenderId = apiCard.CustomFieldItems.FirstOrDefault(x => x.CustomFieldId.IsEqualTo(tenderId.Id));
            var cardLotNumber = apiCard.CustomFieldItems.FirstOrDefault(x => x.CustomFieldId.IsEqualTo(lotNumber.Id));

            return cardTenderId != null && cardLotNumber != null &&
                   tender.TenderId.ToString().IsEqualTo(cardTenderId.Value.TextAsString) &&
                   tender.LotNumber.IsEqualTo(cardLotNumber.Value.TextAsString);
        }
    }
}
