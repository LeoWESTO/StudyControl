using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.ViewModels
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TeacherFullName { get; set; }
        public string ControlType { get; set; }
        public int TermNumber { get; set; }
        public int AreaId { get; set; }
    }
}
