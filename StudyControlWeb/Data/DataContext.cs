using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Models;
using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Faculty> Faculty { get; set;}
        public DbSet<Group> Group { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Cell> Cell { get; set; }
        public DbSet<Points> Point { get; set; }
        public DbSet<Admin> Admin { get; set; }

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
