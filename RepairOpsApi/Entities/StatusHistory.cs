using System.ComponentModel.DataAnnotations;

namespace RepairOpsApi.Entities;

public class StatusHistory
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CaseId { get; set; } 
    public string OldStatus { get; set; }
    public string NewStatus { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

}