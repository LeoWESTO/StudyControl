using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models.DBO
{
    public class Department : BaseModel 
    {
        public string Title { get; set; }
        public string Password { get; set; }
        public int? FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual IEnumerable<Group> Groups { get; set; }
        public virtual IEnumerable<Area> Areas { get; set; }
        public virtual IEnumerable<Teacher> Teachers { get; set; }
    }
}
