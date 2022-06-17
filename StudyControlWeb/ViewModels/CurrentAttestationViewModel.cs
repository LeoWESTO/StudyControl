namespace StudyControlWeb.ViewModels
{
    public class CurrentAttestationViewModel
    {
        public int Id { get; set; }
        public int TermNumber { get; set; }
        public string StudentName { get; set; }
        public string SubjectTitle { get; set; }
        public int Performance { get; set; }
        public int Attendance { get; set; }
        public DateTime Date { get; set; }
    }
}
