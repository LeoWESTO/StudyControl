using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models.DBO
{
    public class Teacher : BaseModel
    {
        public string Name { get; set;}
        public string Surname { get; set;}
        public string Fathername { get; set;}
        public string Password { get; set; }
        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set;}
        public virtual IEnumerable<Subject> Subjects { get; set;}
        public virtual IEnumerable<Cell> Cells { get; set; }
    }
}
