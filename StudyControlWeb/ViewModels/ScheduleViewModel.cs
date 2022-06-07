using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.ViewModels
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public string GroupCode { get; set; }
        public string FacultyTitle { get; set; }
        public int TermNumber { get; set; }
        public List<CellViewModel> Cells { get; set; } = new List<CellViewModel>();
    }
}
