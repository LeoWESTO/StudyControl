﻿namespace StudyControlWeb.Models.DBO
{
    public class Schedule : BaseModel
    {
        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
}
