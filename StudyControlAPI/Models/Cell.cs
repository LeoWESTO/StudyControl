using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlAPI.Models
{
    public enum LessonType
    {
        Lecture,
        Seminar,
        Lab
    }
    public class Cell : BaseModel 
    {
        [JsonIgnore]
        public Group Group { get; set; }
        public int GroupId { get; set; }
        [JsonIgnore]
        public Subject Subject { get; set; }
        public int SubjectId { get; set; }
        [JsonIgnore]
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }

        public DateTime? Date { get; set; }
        public LessonType? LessonType { get; set; }
        public string? Classroom { get; set; }
    }
}
