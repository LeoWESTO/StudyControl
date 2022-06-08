namespace StudyControlWeb.Models.DBO
{
    public class FinalAttestation : BaseModel
    {
        public virtual Student Student { get; set; }
        public int StudentId { get; set; }
        public string Type { get; set; }
        public int Performance { get; set; }
        public DateTime Date { get; set; }
    }
}
