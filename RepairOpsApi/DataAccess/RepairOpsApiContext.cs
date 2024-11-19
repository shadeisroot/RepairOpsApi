using Microsoft.EntityFrameworkCore;
using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess;

public class RepairOpsApiContext : DbContext
{
    public RepairOpsApiContext(DbContextOptions<RepairOpsApiContext> options) : base(options)
    {
        
    }
    
    public DbSet<Sag> Sag { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Sørg for at Sagsnummer er en primær nøgle og korrekt konfigureret
        modelBuilder.Entity<Sag>()
            .HasKey(s => s.Sagsnummer);

        // Hvis nødvendigt, kan du også sikre, at Guid genereres som uniqueidentifier
        modelBuilder.Entity<Sag>()
            .Property(s => s.Sagsnummer)
            .HasDefaultValueSql("NEWID()"); // Dette kan generere et nyt Guid automatisk ved oprettelse
    }
}