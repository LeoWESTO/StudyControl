namespace StudyControlWeb.Models.DBO
{
    public class Plan : BaseModel
    {
        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual IEnumerable<Area> Areas { get; set; }
    }
}
