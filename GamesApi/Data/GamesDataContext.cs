using GamesApi.Data.Mapping;
using GamesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesApi.Data
{
    public class GamesDataContext : DbContext
    {
        public GamesDataContext(DbContextOptions<GamesDataContext> options) : base(options) 
        { 
        }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameMap());
        }
    }
}
