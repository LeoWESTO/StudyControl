using StudyControlWeb.Data.Interfaces;
using StudyControlWeb.Models.DBO;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace StudyControlWeb.Data.Documents
{
    public class ScheduleDocument : IDocument
    {
        public BaseModel Model { get; set; }
        public ScheduleDocument(Schedule model)
        {
            Model = model;
        }
        public MemoryStream GetWord()
        {
            throw new NotImplementedException();
        }
        public MemoryStream GetExcel()
        {
            throw new NotImplementedException();
        }
        public MemoryStream GetWordByPattern(string wordPatternPath)
        {
            throw new NotImplementedException();
        }
        public MemoryStream GetExcelByPattern(string excelPatternPath)
        {
            throw new NotImplementedException();
        }
    }
}
