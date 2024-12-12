using System.ComponentModel.DataAnnotations;

namespace RepairOpsApi.Entities;

public class StatusHistory
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CaseId { get; set; } // Fremmednøgle til Case
    public string OldStatus { get; set; }
    public string NewStatus { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    public Case Case { get; set; } // Navigationsejendom
}