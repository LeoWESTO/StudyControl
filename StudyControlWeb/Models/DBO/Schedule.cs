namespace StudyControlWeb.Models.DBO
{
    public class Schedule : BaseModel
    {
        public int TermNumber { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
}
