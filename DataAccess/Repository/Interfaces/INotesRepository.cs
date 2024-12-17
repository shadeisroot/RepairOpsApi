using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess.Repository.Interfaces;

public interface INotesRepository
{
    Task<IEnumerable<Notes>> GetnotesByCaseIdAsync(Guid caseId);
    Task AddnotesMessageAsync(Notes notes);
}