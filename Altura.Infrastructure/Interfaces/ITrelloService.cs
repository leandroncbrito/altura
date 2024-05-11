using Altura.Domain.Entities;

namespace Altura.Application.Interfaces
{
    public interface ITrelloService
    {
        Task TransformTendersToCards(IEnumerable<Tender> tenders, CancellationToken cancellationToken);
    }
}