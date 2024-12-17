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
    //henter alle cases fra databasen 
    public async Task<IEnumerable<Case>> GetAllCasesAsync()
    {
        return await _context.Cases.ToListAsync(); //retunere en liste af cases
    }
    //henter en eneklt case fra databasen ud fra id
    public async Task<Case> GetCaseByIdAsync(Guid id)
    {
        return await _context.Cases.FindAsync(id); //finder en case ved hjælp af en primarykey (id)
    }
    //tilføjer en ny case til databasen
    public async Task<Case> AddCaseAsync(Case caseItem)
    {
        _context.Cases.Add(caseItem); //tilføjer case til DBSet
        await _context.SaveChangesAsync(); //gemmer ændring i database
        return caseItem; //returnere den nye case
        
    }
    //opdatere en allerede lavet case i databasen
    public async Task<bool> UpdateCaseAsync(Case caseItem)
    {
        // Find den eksisterende sag i databasen
        var existingCase = await _context.Cases.AsNoTracking().FirstOrDefaultAsync(c => c.Id == caseItem.Id);

        if (existingCase == null)
        {
            return false; // Sagen blev ikke fundet
        }

        // Tjek, om status er ændret
        if (existingCase.Status != caseItem.Status)
        {
            // Opret en historikpost for statusændringen
            var statusHistory = new StatusHistory
            {
                CaseId = caseItem.Id,
                OldStatus = existingCase.Status,
                NewStatus = caseItem.Status,
                ChangedAt = DateTime.UtcNow
            };

            // Tilføj historikken til databasen
            _context.StatusHistories.Add(statusHistory);
        }

        // Marker sagen som ændret
        _context.Entry(caseItem).State = EntityState.Modified;

        try
        {
            // Gem ændringerne i databasen
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            // Tjek, om sagen stadig eksisterer
            if (!await _context.Cases.AnyAsync(e => e.Id == caseItem.Id))
            {
                return false; // Sagen blev ikke fundet
            }

            throw; // Genkast fejlen, hvis det ikke var en "sletning"
        }
    }
    //sletter en case fra databasen baseret på dens ID
    public async Task<bool> DeleteCaseAsync(Guid id)
    {
        var caseItem = await _context.Cases
            .Include(c => c.ChatMessages)
            .Include(c => c.Notes)
            .Include(c => c.StatusHistories)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (caseItem == null)
        {
            return false;
        }

        _context.Cases.Remove(caseItem);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<IEnumerable<StatusHistory>> GetStatusHistoryByCaseIdAsync(Guid caseId)
    {
        return await _context.StatusHistories
            .Where(h => h.CaseId == caseId)
            .OrderBy(h => h.ChangedAt)
            .ToListAsync();
    }

}