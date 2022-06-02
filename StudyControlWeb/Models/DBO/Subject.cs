using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models.DBO
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
        public bool IsCoursework { get; set; }
        public int? TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
