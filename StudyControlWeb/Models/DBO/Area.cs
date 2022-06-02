namespace StudyControlWeb.Models.DBO
{
    public enum Degree
    {
        Bachelor = 3,
        Master,
        Specialist,
        Postgraduate
    }
    public class Area : BaseModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public Degree Degree { get; set; }
        public bool IsActive { get; set; }
        public virtual IEnumerable<Group> Groups { get; set; }
    }
}
