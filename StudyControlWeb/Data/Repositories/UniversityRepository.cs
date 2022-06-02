﻿using StudyControlWeb.Data.Interfaces;
using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.Data.Repositories
{
    public class UniversityRepository
    {
        public IRepository<Student> Students { get; set; }
        public IRepository<Teacher> Teachers { get; set; }
        public IRepository<Department> Departments { get; set; }
        public IRepository<Faculty> Faculties { get; set; }
        public IRepository<Admin> Admins { get; set; }
        public IRepository<Group> Groups { get; set; }
        public IRepository<Cell> Cells { get; set; }
        public IRepository<Points> Points { get; set; }
        public IRepository<Area> Areas { get; set; }

        public UniversityRepository(DataContext context)
        {
            Faculties = new Repository<Faculty>(context);
            Departments = new Repository<Department>(context);
            Teachers = new Repository<Teacher>(context);
            Students = new Repository<Student>(context);
            Admins = new Repository<Admin>(context);
            Groups = new Repository<Group>(context);
            Cells = new Repository<Cell>(context);
            Points = new Repository<Points>(context);
            Areas = new Repository<Area>(context);
        }
    }
}