using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models.DBO
{
    public class Student : BaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fathername { get; set; }
        public string Password { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public virtual IEnumerable<CurrentAttestation> CurrentAttestations { get; set; }
        public virtual IEnumerable<IntermediateAttestation> IntermediateAttestations { get; set; }
        public virtual IEnumerable<FinalAttestation> FinalAttestations { get; set; }
    }
}
