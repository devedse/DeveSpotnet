using DeveSpotnet.Db.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DeveSpotnet.Db
{
    public class DeveSpotnetDbContext : DbContext
    {
        public DbSet<DbSpot> Spots => Set<DbSpot>();

        public DeveSpotnetDbContext(DbContextOptions<DeveSpotnetDbContext> options) : base(options)
        {
        }
    }
}
