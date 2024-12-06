using Microsoft.EntityFrameworkCore;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess.Repository;

public class ChatRepository : IChatRepository
{
    private readonly RepairOpsApiContext _context;
    
    public ChatRepository(RepairOpsApiContext context)
    {
        _context = context;
    }
    //henter alle beskeder for sag ud fra caseId
    public async Task<IEnumerable<ChatMessage>> GetChatsByCaseIdAsync(Guid caseId)
    {
        return await _context.ChatMessages
            .Where(c => c.CaseId == caseId)//filtrering
            .ToListAsync();//konventer til liste og udfører
    }
    //Tilføj besked til database
    public async Task AddChatMessageAsync(ChatMessage chatMessage)
    {
        await _context.ChatMessages.AddAsync(chatMessage); //tilføj besked til DBSet
        await _context.SaveChangesAsync(); //gemmer i database
    }
}