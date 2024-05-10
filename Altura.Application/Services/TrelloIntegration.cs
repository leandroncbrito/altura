using Altura.Application.Interfaces;
using Altura.Domain;
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

        private string BoardId = "N20YA7Aj";

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
                var board = await _trelloBoard.ObtainBoardAsync(BoardId, cancellationToken);

                await _trelloCustomFields.InitializeCustomFieldsOnABoard(board.Id, cancellationToken);

                var lists = await _trelloBoard.GetListsOnBoardAsync(board.Id, cancellationToken);

                var listsMap = new Dictionary<int, string>();

                var limit = 1;

                foreach (var tender in tenders)
                {
                    string listId;

                    var status = tender.Status;

                    if (listsMap.ContainsKey(status) is false)
                    {
                        var listName = $"Status:{status}";

                        var list = lists.FirstOrDefault(x => x.Name.IsEqualTo(listName));

                        list ??= await _trelloList.AddListAsync(new List(listName, board.Id), cancellationToken);

                        listsMap.Add(status, list.Id);
                    }

                    listId = listsMap[status];

                    var customFields = await _trelloCustomFields.GetCustomFieldsAsync(board.Id, cancellationToken);

                    var tenderId = customFields.First(x => x.Name.IsEqualTo("TenderId"));
                    var lotNumber = customFields.First(x => x.Name.IsEqualTo("LotNumber"));

                    var apiCards = await _trelloCard.GetCardsInListAsync(listId, cancellationToken);

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

                                await _trelloCard.UpdateCardAsync(card, tender, cancellationToken);

                                break;
                            }
                        }
                    }

                    if (card == null)
                    {
                        await _trelloCard.CreateCardAsync(listId, tender, cancellationToken);
                    }

                    limit--;
                    if (limit == 0)
                    {
                        break;
                    }

                    //return true;
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError("Error transforming tenders to cards: " + ex.Message, ex);

                throw;
            }
        }
    }
}
