using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlAPI.Models
{
    public class Dean : BaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fathername { get; set; }
        [JsonIgnore]
        public Faculty Faculty { get; set; }
        public int? FacultyId { get; set; }
    }
}
