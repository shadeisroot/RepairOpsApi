using Microsoft.EntityFrameworkCore;

namespace RepairOpsApi.DataAccess;

public class RepairOpsApiContext : DbContext
{
    public RepairOpsApiContext(DbContextOptions<RepairOpsApiContext> options) : base(options)
    {
        
    }
}