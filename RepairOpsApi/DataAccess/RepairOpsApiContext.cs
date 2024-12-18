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
    
    public DbSet<Notes> Notes { get; set; }
    
    public DbSet<StatusHistory> StatusHistories { get; set; }

    public RepairOpsApiContext(DbContextOptions<RepairOpsApiContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Notes>()
            .HasOne(n => n.Case)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}