using Microsoft.EntityFrameworkCore;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess.Repository;

public class CaseRepository : ICaseRepository
{
    private readonly RepairOpsApiContext _context;

    public CaseRepository(RepairOpsApiContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Case>> GetAllCasesAsync()
    {
        return await _context.Cases.ToListAsync();
    }

    public async Task<Case> GetCaseByIdAsync(Guid id)
    {
        return await _context.Cases.FindAsync(id);
    }

    public async Task<Case> AddCaseAsync(Case caseItem)
    {
        _context.Cases.Add(caseItem);
        await _context.SaveChangesAsync();
        return caseItem;
    }

    public async Task<bool> UpdateCaseAsync(Case caseItem)
    {
        _context.Entry(caseItem).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Cases.AnyAsync(e => e.Id == caseItem.Id))
            {
                return false;
            }
            throw;
        }
    }

    public async Task<bool> DeleteCaseAsync(Guid id)
    {
        var caseItem = await _context.Cases.FindAsync(id);
        if (caseItem == null) return false;

        _context.Cases.Remove(caseItem);
        await _context.SaveChangesAsync();
        return true;
    }
}