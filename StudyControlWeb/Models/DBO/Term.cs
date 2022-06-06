using System.ComponentModel.DataAnnotations;

namespace StudyControlWeb.Models.DBO
{
    public class Term : BaseModel
    {
        public int Number { get; set; }
        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual IEnumerable<Subject> Subjects { get; set; }
    }
}
