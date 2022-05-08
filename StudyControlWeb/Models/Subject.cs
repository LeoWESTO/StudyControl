using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models
{
    public enum ControlType
    {
        Test,
        Exam
    }
    public class Subject : BaseModel
    {
        public string Title { get; set; }
        public ControlType? ControlType { get; set; }
        [JsonIgnore]
        public Teacher Teacher { get; set; }
        public int? TeacherId { get; set; }
    }
}
