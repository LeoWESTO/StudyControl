using System.ComponentModel.DataAnnotations;

namespace StudyControlWeb.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите свою фамилию")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Введите свое имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите свое отчество")]
        public string Fathername { get; set; }

        [Required(ErrorMessage = "Введите свой пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}