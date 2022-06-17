using StudyControlWeb.Data.Interfaces;
using StudyControlWeb.Models.DBO;
using StudyControlWeb.ViewModels;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace StudyControlWeb.Data.Documents
{
    public class DocumentManager
    {
        private Dictionary<string, string> _replaces;
        public DocumentManager()
        {

        }

        public MemoryStream GetScheule(string excelPatternPath)
        {
            throw new NotImplementedException();
        }

        public MemoryStream GetStudentsIntermediate(FileInfo wordPattern, PerformanceViewModel model)
        {
            _replaces = new Dictionary<string, string>()
            {
                { "{Group}", model.Group },
                { "{StudentFullName}", model.StudentFullName },
                { "{FacultyTitle}", model.FacultyTitle },
                { "{DepartmentTitle}", model.DepartmentTitle },
                { "{StudyYearStart1}", model.DepartmentTitle },
                { "{StudyYearEnd1}", model.DepartmentTitle },
                { "{StudyYearStart2}", model.DepartmentTitle },
                { "{StudyYearEnd2}", model.DepartmentTitle },
                { "{StudyYearStart3}", model.DepartmentTitle },
                { "{StudyYearEnd3}", model.DepartmentTitle },
                { "{StudyYearStart4}", model.DepartmentTitle },
                { "{StudyYearEnd4}", model.DepartmentTitle },
                { "{StudyYearStart5}", model.DepartmentTitle },
                { "{StudyYearEnd5}", model.DepartmentTitle },
            };

            Word.Application application = null;
            string newFileName = "";

            try
            {
                application = new Word.Application();

                newFileName = Path.Combine(wordPattern.DirectoryName, DateTime.Now.ToString("yyyyMMddHHmmss ") + wordPattern.Name);
                File.Copy(wordPattern.FullName, newFileName);

                object file = newFileName;
                object missing = Type.Missing;
                application.Documents.Open(file);
                foreach (var item in _replaces)
                {
                    Word.Find find = application.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    object wrap = Word.WdFindWrap.wdFindContinue;
                    object replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(
                        Type.Missing,
                        false,
                        false,
                        false,
                        missing,
                        false,
                        true,
                        wrap,
                        false,
                        missing,
                        replace);
                }

                for (int term = 1; term <= 10; term++)
                {
                    var sub = model.IntermediateAttestations.Where(i => i.Subject.TermNumber == term);
                    if (sub.Any())
                    {
                        for (int row = 4; row <= sub.Count() + 4; row++)
                        {
                            application.ActiveDocument.Tables[term].Cell(row, 1).Range.Text = term.ToString();
                        }
                        
                    } 
                }

                application.ActiveDocument.Save();
                application.ActiveDocument.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally
            {
                if (application != null)
                {
                    application.Quit();
                }
            }
            var stream = new MemoryStream();
            using (FileStream fs = File.OpenRead(newFileName))
            {
                fs.CopyTo(stream);
            }
            stream.Position = 0;
            File.Delete(newFileName);
            return stream;
        }
    }
}
