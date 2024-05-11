using Altura.Domain.Entities;

namespace Altura.Application.Interfaces
{
    public interface ITrelloTenderService
    {
        Task TransformTendersToCards(IEnumerable<Tender> tenders, CancellationToken cancellationToken);
    }
}