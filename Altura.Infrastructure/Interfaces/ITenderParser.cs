using Altura.Domain.Entities;

namespace Altura.Infrastructure.Interfaces
{
    public interface ITenderParser
    {
        IEnumerable<Tender> ParseTenders();
    }
}
