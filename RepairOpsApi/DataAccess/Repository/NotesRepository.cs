using Microsoft.EntityFrameworkCore;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess.Repository;

public class NotesRepository : INotesRepository
{
    
    private readonly RepairOpsApiContext _context;
    
    public NotesRepository(RepairOpsApiContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Notes>> GetnotesByCaseIdAsync(Guid caseId)
    {
        return await _context.Notes
            .Where(c => c.CaseId == caseId)//filtrering
            .ToListAsync();//konventer til liste og udfører
    }

    public async Task AddnotesMessageAsync(Notes notes)
    {
        await _context.Notes.AddAsync(notes); //tilføj besked til DBSet
        await _context.SaveChangesAsync(); //gemmer i database
    }
}