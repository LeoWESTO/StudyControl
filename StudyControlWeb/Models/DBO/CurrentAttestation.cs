namespace StudyControlWeb.Models.DBO
{
    public class CurrentAttestation : BaseModel
    {
        public virtual Student Student { get; set; }
        public int StudentId { get; set; }
        public virtual Subject Subject { get; set; }
        public int SubjectId { get; set; }
        public int Performance { get; set; }
        public int Attendance { get; set; }
        public DateTime Date { get; set; }
    }
}
