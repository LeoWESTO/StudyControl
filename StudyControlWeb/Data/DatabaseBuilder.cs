﻿using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.Data
{
    public static class DatabaseBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            AdminBuild(modelBuilder);
            FacultyBuild(modelBuilder);
            DepartmentBuild(modelBuilder);
            TeacherBuild(modelBuilder);
            StudentBuild(modelBuilder);
            AreaBuild(modelBuilder);
            RoleBuild(modelBuilder);
        }
        private static void AdminBuild(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Login = "admin",
                    Password = "1"
                }
            );
        }
        private static void FacultyBuild(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>().HasData(
                new Faculty
                {
                    Id = 1,
                    Title = "Факультет компьютерных технологий, вычислительной техники и энергетики",
                    Password = "1"
                },
                new Faculty
                {
                    Id = 2,
                    Title = "Архитектурно-строительный факультет",
                    Password = "1"
                },
                new Faculty
                {
                    Id = 3,
                    Title = "Факультет информационных систем в экономике и управлении",
                    Password = "1"
                },
                new Faculty
                {
                    Id = 4,
                    Title = "Факультет радиоэлектроники, телекоммуникаций и мультимедийных технологий",
                    Password = "1"
                },
                new Faculty
                {
                    Id = 5,
                    Title = "Технологический факультет",
                    Password = "1"
                },
                new Faculty
                {
                    Id = 6,
                    Title = "Факультет нефти, газа и природообустройства ",
                    Password = "1"
                },
                new Faculty
                {
                    Id = 7,
                    Title = "Факультет права и управления на транспорте",
                    Password = "1"
                },
                new Faculty
                {
                    Id = 8,
                    Title = "Факультет дополнительного образования и профессионального обучения",
                    Password = "1"
                },
                new Faculty
                {
                    Id = 9,
                    Title = "Факультет магистерской подготовки",
                    Password = "1"
                },
                new Faculty
                {
                    Id = 10,
                    Title = "Факультет среднего профессионального образования",
                    Password = "1"
                }
            );
        }
        private static void DepartmentBuild(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department
                {
                    Id = 1,
                    FacultyId = 1,
                    Title = "Кафедра электроэнергетики и возобновляемых источников энергии",
                    Password = "1",
                },
                new Department
                {
                    Id = 2,
                    FacultyId = 1,
                    Title = "Кафедра прикладной математики и информатики",
                    Password = "1",
                },
                new Department
                {
                    Id = 3,
                    FacultyId = 1,
                    Title = "Кафедра программного обеспечения вычислительной техники и автоматизированных систем",
                    Password = "1",
                },
                new Department
                {
                    Id = 4,
                    FacultyId = 1,
                    Title = "Кафедра теоретической и общей электротехники",
                    Password = "1",
                },
                new Department
                {
                    Id = 5,
                    FacultyId = 1,
                    Title = "Кафедра управления и информатики в технических системах и вычислительной техники",
                    Password = "1",
                },
                new Department
                {
                    Id = 6,
                    FacultyId = 1,
                    Title = "Кафедра информационной безопасности",
                    Password = "1",
                }
            );
        }
        private static void TeacherBuild(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher
                {
                    Id = 1,
                    DepartmentId = 3,
                    Surname = "Айгумов",
                    Name = "Тимур",
                    Fathername = "Гаджиевич",
                    Password = "1"
                }
            );
        }
        private static void StudentBuild(ModelBuilder modelBuilder)
        {

        }
        private static void AreaBuild(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>().HasData(
                new Area
                {
                    Id = 1,
                    Code = "09.03.01",
                    Title = "Информатика и вычислительная техника",
                    Degree = Degree.Bachelor,
                    IsActive = true,
                },
                new Area
                {
                    Id = 2,
                    Code = "09.03.02",
                    Title = "Информационные системы и технологии",
                    Degree = Degree.Bachelor,
                    IsActive = true,
                },
                new Area
                {
                    Id = 3,
                    Code = "09.03.03",
                    Title = "Прикладная информатика",
                    Degree = Degree.Bachelor,
                    IsActive = true,
                },
                new Area
                {
                    Id = 4,
                    Code = "09.03.04",
                    Title = "Программная инженерия",
                    Degree = Degree.Bachelor,
                    IsActive = true,
                },
                new Area
                {
                    Id = 5,
                    Code = "10.03.01",
                    Title = "Информационная безопасность",
                    Degree = Degree.Bachelor,
                    IsActive = true,
                }
            );
        }
        private static void RoleBuild(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                },
                new Role
                {
                    Id = 2,
                    Name = "Faculty",
                },
                new Role
                {
                    Id = 3,
                    Name = "Department",
                },
                new Role
                {
                    Id = 4,
                    Name = "Teacher",
                },
                new Role
                {
                    Id = 5,
                    Name = "Student",
                }
            );
        }
    }
}