using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models
{
    public class Department : BaseModel 
    {
        public string Title { get; set; }
        [JsonIgnore]
        public Faculty Faculty { get; set; }
        public int? FacultyId { get; set; }
    }
}
