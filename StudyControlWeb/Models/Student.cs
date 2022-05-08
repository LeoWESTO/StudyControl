using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models
{
    public class Student : BaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fathername { get; set; }
        [JsonIgnore]
        public Group Group { get; set; }
        public int? GroupId { get; set; }
    }
}
