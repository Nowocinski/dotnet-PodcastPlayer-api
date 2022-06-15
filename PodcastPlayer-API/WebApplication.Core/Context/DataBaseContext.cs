using Microsoft.EntityFrameworkCore;
using WebApplication.Core.Models;

namespace WebApplication.Core.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(column =>
            {
                column.HasKey(key => key.Id);
            });
        }

        public DbSet<User> Users { get; set; }
    }
}
