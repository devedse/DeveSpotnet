using DeveSpotnet.Db.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DeveSpotnet.Db
{
    public class DeveSpotnetDbContext : DbContext
    {
        public DbSet<DbSpotHeader> SpotHeaders => Set<DbSpotHeader>();

        public DeveSpotnetDbContext(DbContextOptions<DeveSpotnetDbContext> options) : base(options)
        {
        }
    }
}
