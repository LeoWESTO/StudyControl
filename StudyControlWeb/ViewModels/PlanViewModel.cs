using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.ViewModels
{
    public class PlanViewModel
    {
        public int Id { get; set; }
        public Area Area { get; set; }
        public List<SubjectViewModel> SubjectViewModels { get; set; } = new List<SubjectViewModel>();
    }
}
