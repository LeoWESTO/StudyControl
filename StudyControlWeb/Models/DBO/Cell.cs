using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models.DBO
{
    public class Cell : BaseModel 
    {
        public virtual Group Group { get; set; }
        public int GroupId { get; set; }
        public virtual Subject Subject { get; set; }
        public int SubjectId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
        public virtual Schedule Schedule { get; set; }
        public int ScheduleId { get; set; }

        public int WeekNumber { get; set; }
        public int LessonNumber { get; set; }
        public int DayOfWeek { get; set; }
        public string? LessonType { get; set; }
        public string? Classroom { get; set; }
    }
}
