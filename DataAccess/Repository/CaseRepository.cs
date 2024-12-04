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
        _context.Entry(caseItem).State = EntityState.Modified; //marker case som ændret
        try
        {
            await _context.SaveChangesAsync(); //gemmer ændring i database
            return true; //returnere true hvis det lykkes og opdatere
        }
        catch (DbUpdateConcurrencyException) //håndtere en fejl i database hvis rækken som ændres ikke matcher med det som der forventes
        {
            //tjekker om case stadig eksisterer 
            if (!await _context.Cases.AnyAsync(e => e.Id == caseItem.Id))
            {
                return false; //returnere false hvis den ikke findes 
            }

            throw;
        }
    }
    //sletter en case fra databasen baseret på dens ID
    public async Task<bool> DeleteCaseAsync(Guid id)
    {
        var caseToDelete = await _context.Cases.FindAsync(id);
        if (caseToDelete == null) return false;

        _context.Cases.Remove(caseToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

}