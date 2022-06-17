using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.ViewModels
{
    public class PerformanceViewModel : BaseViewModel
    {
        public string Group { get; set; }
        public string FacultyTitle { get; set; }
        public string DepartmentTitle { get; set; }
        public string StudentFullName { get; set; }
        public List<StudyYear> StudyYears { get; set; }
        public List<IntermediateAttestation> IntermediateAttestations { get; set; }
    }
}
