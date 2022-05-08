using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models
{
    public class Teacher : BaseModel
    {
        public string Name { get; set;}
        public string Surname { get; set;}
        public string Fathername { get; set;}
        [JsonIgnore]
        public Department Department { get; set;}
        public int? DepartmentId { get; set; }
        public List<Subject> Subjects { get; set;}
    }
}
