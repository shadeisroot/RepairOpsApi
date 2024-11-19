using Microsoft.EntityFrameworkCore;
using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess;

public class RepairOpsApiContext : DbContext
{
    public DbSet<User> Users {get; set;}
    public RepairOpsApiContext(DbContextOptions<RepairOpsApiContext> options) : base(options)
    {
        
    }
}