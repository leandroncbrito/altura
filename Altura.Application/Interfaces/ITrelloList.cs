using TrelloDotNet.Model;

namespace Altura.Application.Interfaces
{
    public interface ITrelloList
    {
        Task<List> AddListAsync(List list, CancellationToken cancellationToken);
    }
}