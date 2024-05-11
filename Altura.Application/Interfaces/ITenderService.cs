namespace Altura.Application.Interfaces
{
    public interface ITenderService
    {
        Task<bool> ExtractTendersFromCsv(CancellationToken cancellationToken);
    }
}
