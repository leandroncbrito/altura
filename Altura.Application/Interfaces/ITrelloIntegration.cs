using Altura.Domain;

namespace Altura.Application.Interfaces
{
    public interface ITrelloIntegration
    {
        Task TransformTendersToCards(IEnumerable<Tender> tenders, CancellationToken cancellationToken);
    }
}