using RepairOpsApi.Entities;

namespace RepairOpsApi;

public interface ISagRepository
{
    Task<Sag> GetSagAsync(Guid id);
    Task<bool> SagExistsAsync(Guid sagsnummer);
    Task<Sag> CreateSagAsync(Sag sag);
    Task SaveChangesAsync();
}