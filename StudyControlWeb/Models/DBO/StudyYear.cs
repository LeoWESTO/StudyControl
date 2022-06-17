namespace StudyControlWeb.Models.DBO
{
    public class StudyYear : BaseModel
    {
        public DateTime? StartAutumnTerm { get; set; }
        public DateTime? EndAutumnTerm { get; set; }
        public DateTime? StartSpringTerm { get; set; }
        public DateTime? EndSpringTerm { get; set; }
    }
}
