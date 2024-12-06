namespace RepairOpsApi.Entities;

public class ChatMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CaseId { get; set; } 
    public string Sender { get; set; } 
    public string Message { get; set; } 
    public DateTime Timestamp { get; set; } = DateTime.UtcNow; 
    

}