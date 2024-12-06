using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess.Repository.Interfaces;

public interface IChatRepository
{
    Task<IEnumerable<ChatMessage>> GetChatsByCaseIdAsync(Guid caseId);
    Task AddChatMessageAsync(ChatMessage chatMessage);
}