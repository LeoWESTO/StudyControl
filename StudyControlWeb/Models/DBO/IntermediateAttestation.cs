namespace StudyControlWeb.Models.DBO
{
    public class IntermediateAttestation : BaseModel
    {
        public virtual Student Student { get; set; }
        public int StudentId { get; set; }
        public virtual Subject Subject { get; set; }
        public int SubjectId { get; set; }
        public string ControlType { get; set; }
        public int Performance { get; set; }
        public DateTime Date { get; set; }
    }
}
