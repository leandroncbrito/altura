using Altura.Application.Interfaces;
using Altura.Infrastructure.Interfaces;

namespace Altura.Application.Services
{
    public class TenderService : ITenderService
    {
        private readonly ITenderParser _tenderParser;
        private readonly ITrelloTenderService _trelloTenderService;

        public TenderService(ITenderParser tenderExtractor, ITrelloTenderService trelloTenderService)
        {
            _tenderParser = tenderExtractor;
            _trelloTenderService = trelloTenderService;
        }

        public async Task<bool> ExtractTendersFromCsv(CancellationToken cancellationToken)
        {
            var tenders = _tenderParser.ParseTenders();

            await _trelloTenderService.TransformTendersToCards(tenders, cancellationToken);

            return true;
        }
    }
}
