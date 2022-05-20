using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Models;

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

        public DataContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>().HasData(
                    new Faculty
                    {
                        Id = 1, 
                        Title = "Факультет компьютерных технологий, вычислительной техники и энергетики",
                        DeanName = "Ширали",
                        DeanSurname = "Юсуфов",
                        DeanFathername = "Абдулкадиевич",
                        Password = "1"
                    },
                    new Faculty 
                    { 
                        Id = 2, 
                        Title = "Архитектурно-строительный факультет",
                        DeanName = "Тагир",
                        DeanSurname = "Азаев",
                        DeanFathername = "Магомедович",
                        Password = "1"
                    },
                    new Faculty 
                    { 
                        Id = 3, 
                        Title = "Факультет информационных систем в экономике и управлении",
                        DeanName = "Земфира",
                        DeanSurname = "Раджабова",
                        DeanFathername = "Рамазановна",
                        Password = "1"
                    },
                    new Faculty 
                    { 
                        Id = 4, 
                        Title = "Факультет радиоэлектроники, телекоммуникаций и мультимедийных технологий",
                        DeanName = "Гюльнара",
                        DeanSurname = "Кардашова",
                        DeanFathername = "Дарвиновна",
                        Password = "1"
                    },
                    new Faculty 
                    { 
                        Id = 5, 
                        Title = "Технологический факультет",
                        DeanName = "Фаина",
                        DeanSurname = "Азимова",
                        DeanFathername = "Шамиловна",
                        Password = "1"
                    },
                    new Faculty 
                    { 
                        Id = 6, 
                        Title = "Факультет нефти, газа и природообустройства ",
                        DeanName = "Милада",
                        DeanSurname = "Магомедова",
                        DeanFathername = "Руслановна",
                        Password = "1"
                    },
                    new Faculty 
                    { 
                        Id = 7, 
                        Title = "Факультет права и управления на транспорте",
                        DeanName = "Эдвард",
                        DeanSurname = "Батманов",
                        DeanFathername = "Загидинович",
                        Password = "1"
                    },
                    new Faculty 
                    { 
                        Id = 8, 
                        Title = "Факультет дополнительного образования и профессионального обучения",
                        DeanName = "Айшат",
                        DeanSurname = "Шахмаева",
                        DeanFathername = "Расуловна",
                        Password = "1"
                    },
                    new Faculty 
                    { 
                        Id = 9, 
                        Title = "Факультет магистерской подготовки",
                        DeanName = "Румина",
                        DeanSurname = "Ашуралиева",
                        DeanFathername = "Касумовна",
                        Password = "1"
                    },
                    new Faculty
                    {
                        Id = 10,
                        Title = "Факультет среднего профессионального образования",
                        DeanName = "Мадина",
                        DeanSurname = "Абдусаламова",
                        DeanFathername = "Магомеддибировна",
                        Password = "1"
                    }
                );
        }
    }
}
