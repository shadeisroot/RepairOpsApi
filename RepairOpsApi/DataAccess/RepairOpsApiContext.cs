using Microsoft.EntityFrameworkCore;
using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess;

public class RepairOpsApiContext : DbContext
{
    //database set for User
    public DbSet<User> Users {get; set;}
    //database set for Case
    public DbSet<Case> Cases {get; set;}
    //database set for Chat
    public DbSet<ChatMessage> ChatMessages {get; set;}

<<<<<<< HEAD
    public DbSet<Notes> Notes { get; set; }


=======
    public DbSet<StatusHistory> StatusHistories { get; set; }
>>>>>>> fb470e97c51a50c42f28ea3a64c07a5bd2795fb1
    public RepairOpsApiContext(DbContextOptions<RepairOpsApiContext> options) : base(options)
    {
        
    }
    
}