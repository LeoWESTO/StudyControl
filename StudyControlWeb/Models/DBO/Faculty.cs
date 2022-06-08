using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace StudyControlWeb.Models.DBO
{
    public class Faculty : BaseModel 
    {
        public string Title { get; set; }
        public string Password { get; set; }
        public virtual IEnumerable<Department> Departments { get; set; }
        public virtual IEnumerable<Group> Groups { get; set; }
    }
}
