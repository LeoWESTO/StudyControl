namespace StudyControlWeb.ViewModels
{
    public class CellViewModel
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }

        public string GroupCode { get; set; }

        public int WeekNumber { get; set; }
        public int LessonNumber { get; set; }
        public int DayOfWeek { get; set; }
        public string SubjectTitle { get; set; }
        public string ControlType { get; set; }
        public string LessonType { get; set; }
        public string TeacherName { get; set; }
        public string Classroom { get; set; }
    }
}
