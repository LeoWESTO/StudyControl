using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlAPI.Models
{
    public class Group : BaseModel
    {
        public string Code { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }
        public int? DepartmentId { get; set; }
        [JsonIgnore]
        public Student Headman { get; set; }
        public int? HeadmanId { get; set; }
    }
}
