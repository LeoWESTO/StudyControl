using StudyControlWeb.Models.DBO;
using StudyControlWeb.ViewModels;

namespace StudyControlWeb.Data.Interfaces
{
    public interface IDocument
    {
        BaseViewModel ViewModel { get; set; }
        public MemoryStream GetWordByPattern(string wordPatternPath);
        public MemoryStream GetExcelByPattern(string excelPatternPath);
    }
}
