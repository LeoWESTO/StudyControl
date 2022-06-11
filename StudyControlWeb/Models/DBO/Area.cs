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
        public string Profile { get; set; }
        public string Form { get; set; }
        public Degree Degree { get; set; }
        public int TermsCount { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual IEnumerable<Group> Groups { get; set; }
        public virtual IEnumerable<Subject> Subjects { get; set; }
    }
}
