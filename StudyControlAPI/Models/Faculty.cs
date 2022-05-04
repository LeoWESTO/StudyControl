using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlAPI.Models
{
    public class Faculty : BaseModel 
    {
        public string Title { get; set; }
        public int? DeanId { get; set; }
        [JsonIgnore]
        public Dean Dean { get; set; }
    }
}
