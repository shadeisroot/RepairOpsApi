namespace RepairOpsApi.Entities;

public class Notes
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CaseId { get; set; } 
    public string Message { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public Case Case { get; set; } // Navigation property


}