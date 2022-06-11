using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models.DBO
{
    public class Subject : BaseModel
    {
        public string Title { get; set; }
        public string ControlTypes { get; set; }
        public int TermNumber { get; set; }
        public int? TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }
        public virtual IEnumerable<Cell> Cells { get; set; }
        public virtual IEnumerable<CurrentAttestation> CurrentAttestations { get; set; }
        public virtual IEnumerable<IntermediateAttestation> IntermediateAttestations { get; set; }
    }
}
