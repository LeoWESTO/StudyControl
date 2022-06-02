using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyControlWeb.Models.DBO
{
    public class Points : BaseModel
    {
        public int? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public int FirstTest { get; set; }
        public int SecondTest { get; set; }
        public int ThirdTest { get; set; }
        public int Attendance { get; set; }
        public int LastTest { get; set; }
    }
}
