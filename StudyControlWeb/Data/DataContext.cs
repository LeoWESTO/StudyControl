using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Models;
using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DatabaseBuilder.Build(modelBuilder);
        }
    }
}
