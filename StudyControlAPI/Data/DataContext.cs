using Microsoft.EntityFrameworkCore;
using StudyControlAPI.Models;

namespace StudyControlAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Dean> Deans { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set;}
        public DbSet<Group> Groups { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Points> Points { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dean>()
                    .HasOne(d => d.Faculty)
                    .WithOne(f => f.Dean)
                    .HasForeignKey<Faculty>(f => f.DeanId);
            modelBuilder.Entity<Group>()
                    .HasOne(g => g.Headman)
                    .WithOne(s => s.Group)
                    .HasForeignKey<Student>(s => s.Id);
        }
    }
}
