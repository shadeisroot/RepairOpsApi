using Microsoft.EntityFrameworkCore;
using RepairOpsApi.DataAccess;
using RepairOpsApi.Entities;

namespace RepairOpsApi;

public class SagRepository : ISagRepository
{
    private readonly RepairOpsApiContext _context;

    public SagRepository(RepairOpsApiContext context)
    {
        _context = context;
    }
    
    public async Task<Sag> GetSagAsync(Guid id)
    {
        return await _context.Sag.FindAsync(id);
    }

    public async Task<bool> SagExistsAsync(Guid sagsnummer)
    {
        return await _context.Sag.AnyAsync(s => s.Sagsnummer == sagsnummer);
    }

    public async Task<Sag> CreateSagAsync(Sag sag)
    {
        _context.Sag.Add(sag);
        await SaveChangesAsync();
        return sag;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}