using Altura.Application.Interfaces;
using Altura.Infrastructure.Interfaces;

namespace Altura.Application.Services
{
    public class TenderProcessor : ITenderProcessor
    {
        private readonly ITenderParser _tenderParser;
        private readonly ITrelloIntegration _trelloIntegration;

        public TenderProcessor(ITenderParser tenderExtractor, ITrelloIntegration trelloIntegration)
        {
            _tenderParser = tenderExtractor;
            _trelloIntegration = trelloIntegration;
        }

        public async Task<bool> ExtractTendersFromCsv(CancellationToken cancellationToken)
        {            
            var tenders = _tenderParser.ParseTenders();

            await _trelloIntegration.TransformTendersToCards(tenders, cancellationToken);

            return true;
        }
    }
}
