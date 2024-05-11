using Altura.Application.Interfaces;
using Altura.Infrastructure.Interfaces;

namespace Altura.Application.Services
{
    public class TenderService : ITenderService
    {
        private readonly ITenderParser _tenderParser;
        private readonly ITrelloService _trelloService;

        public TenderService(ITenderParser tenderExtractor, ITrelloService trelloTenderService)
        {
            _tenderParser = tenderExtractor;
            _trelloService = trelloTenderService;
        }

        public async Task<bool> ExtractTendersFromCsv(CancellationToken cancellationToken)
        {
            var tenders = _tenderParser.ParseTenders();

            await _trelloService.TransformTendersToCards(tenders, cancellationToken);

            return true;
        }
    }
}
