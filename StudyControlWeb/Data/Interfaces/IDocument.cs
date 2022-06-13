using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.Data.Interfaces
{
    public interface IDocument
    {
        BaseModel Model { get; set; }
        public MemoryStream GetWord();
        public MemoryStream GetExcel();
        public MemoryStream GetWordByPattern(string wordPatternPath);
        public MemoryStream GetExcelByPattern(string excelPatternPath);
    }
}
