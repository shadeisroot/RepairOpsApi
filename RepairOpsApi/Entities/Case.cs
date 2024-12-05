using System.ComponentModel.DataAnnotations;

namespace RepairOpsApi.Entities;

public class Case
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerName { get; set; }
    public string EquipmentType { get; set; }
    public string ProblemDescription { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string Priority { get; set; }
    public string AssignedTechnician { get; set; }
    public string Status { get; set; }
    public string Mail {get; set; }
}