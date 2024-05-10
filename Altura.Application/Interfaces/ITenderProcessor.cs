namespace Altura.Application.Interfaces
{
    public interface ITenderProcessor
    {
        Task<bool> ExtractTendersFromCsv(CancellationToken cancellationToken);
    }
}
