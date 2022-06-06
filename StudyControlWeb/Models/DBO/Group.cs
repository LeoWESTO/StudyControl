using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models.DBO
{
    public class Group : BaseModel
    {
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }
        public virtual IEnumerable<Student> Students { get; set; }
        public virtual IEnumerable<Cell> Cells { get; set; }
    }
}
