using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.ViewModels
{
    public class AreaViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Profile { get; set; }
        public string Form { get; set; }
        public int TermsCount { get; set; }
        public string DepartmentTitle { get; set; }
        public string DegreeName { get; set; }
        public List<SubjectViewModel> Subjects { get; set; } = new List<SubjectViewModel>();
    }
}
