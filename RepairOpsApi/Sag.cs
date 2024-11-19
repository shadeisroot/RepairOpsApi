using System.ComponentModel.DataAnnotations;

namespace RepairOpsApi.Entities;

public class Sag
{
    [Key]
    public Guid Sagsnummer { get; set; }
   
}