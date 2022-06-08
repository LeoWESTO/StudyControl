using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.ViewModels
{
    public class AttestationsViewModel
    {
        public Group Group { get; set; }
        public List<CurrentAttestationViewModel> CurrentAttestations { get; set; } = new List<CurrentAttestationViewModel>();
        public List<IntermediateAttestationViewModel> IntermediateAttestations { get; set; } = new List<IntermediateAttestationViewModel>();
        public List<FinalAttestationViewModel> FinalAttestations { get; set; } = new List<FinalAttestationViewModel>();
    }
}
