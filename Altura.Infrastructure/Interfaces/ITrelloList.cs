using TrelloDotNet.Model;

namespace Altura.Infrastructure.Interfaces
{
    public interface ITrelloList
    {
        Task<List> AddListAsync(List list, CancellationToken cancellationToken);
    }
}