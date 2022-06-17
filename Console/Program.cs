using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
Dictionary<string, string> items = new Dictionary<string, string>()
{
    { "{Group}", "У-833" },
};
Word.Application application = null;
try
{
    application = new Word.Application();
    application.Visible = true;
    object file = new FileInfo("шаблон зачетки.docx").FullName;
    object missing = Type.Missing;
    application.Documents.Open(file);
    foreach (var item in items)
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
    application.ActiveDocument.SaveAs("edit"+file);
    application.ActiveDocument.Close();
}
catch(Exception ex) { Console.WriteLine(ex.Message); }
finally
{
    if (application != null)
    {
        application.Quit();
    }
}

//object newTemplate = false;
//object documentType = Word.WdNewDocumentType.wdNewBlankDocument;
//object visible = true;
//Word.Document document = application.Documents.Add(ref template, ref newTemplate, ref documentType, ref visible);
//document.Activate();

//document.Close();
//application.Quit();