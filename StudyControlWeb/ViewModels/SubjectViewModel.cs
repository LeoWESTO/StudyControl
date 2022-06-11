using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.ViewModels
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TeacherFullName { get; set; }
        public bool IsTest { get; set; }
        public bool IsGradingTest { get; set; }
        public bool IsExam { get; set; }
        public bool IsCourseWork { get; set; }
        public string ControlTypes { get; set; }
        public int TermNumber { get; set; }
        public int AreaId { get; set; }
    }
}
