using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess.Repository.Interfaces;

public interface ICaseRepository
{
    Task<IEnumerable<Case>> GetAllCasesAsync();
    Task<Case> GetCaseByIdAsync(Guid id);
    Task<Case> AddCaseAsync(Case caseItem);
    Task<bool> UpdateCaseAsync(Case caseItem);
    Task<bool> DeleteCaseAsync(Guid id);
    Task<IEnumerable<StatusHistory>> GetStatusHistoryByCaseIdAsync(Guid caseId);
}