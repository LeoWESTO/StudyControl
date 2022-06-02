﻿using System.ComponentModel.DataAnnotations;

namespace StudyControlWeb.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FacultyTitle { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
