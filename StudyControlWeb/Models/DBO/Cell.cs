using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models.DBO
{
    public enum LessonType
    {
        Lecture,
        Seminar,
        Lab
    }
    public class Cell : BaseModel 
    {
        public virtual Group Group { get; set; }
        public int GroupId { get; set; }
        public virtual Subject Subject { get; set; }
        public int SubjectId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int TeacherId { get; set; }

        public int Number { get; set; }
        public DateTime? Date { get; set; }
        public LessonType? LessonType { get; set; }
        public string? Classroom { get; set; }
    }
}
