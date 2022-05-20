using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models
{
    public class Faculty : BaseModel 
    {
        public string Title { get; set; }
        public string Password { get; set; }
        public string DeanName { get; set; }
        public string DeanSurname { get; set; }
        public string DeanFathername { get; set; }
        
    }
}
