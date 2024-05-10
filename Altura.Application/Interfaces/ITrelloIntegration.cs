using Altura.Domain;

namespace Altura.Application.Interfaces
{
    public interface ITrelloIntegration
    {
        Task ExtractTenderProperties(IEnumerable<Tender> tenders, CancellationToken cancellationToken);
    }
}