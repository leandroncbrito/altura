using Altura.Domain;

namespace Altura.Infrastructure.Interfaces
{
    public interface ITenderParser
    {
        IEnumerable<Tender> ParseTenders();
    }
}
