using StudyControlWeb.Models.DBO;
using System.ComponentModel.DataAnnotations;

namespace StudyControlWeb.ViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string DepartmentTitle { get; set; }
        public string AreaTitle { get; set; }
        public string Profile { get; set; }
        public int StartYear { get; set; }
        public List<Student> Students { get; set; }
    }
}
