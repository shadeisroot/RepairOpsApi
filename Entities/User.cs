using System.ComponentModel.DataAnnotations;

namespace RepairOpsApi.Entities;

public class User
{
    [Key]
    public int id{get; set;}
    public string username { get; set; }
    public string Hash { get; set; }
    public string Salt { get; set; }
    
}