using StudyControlWeb.Data.Interfaces;

namespace StudyControlWeb.Data.Documents
{
    public class DocumentManager
    {
        public IDocument Document { get; private set; }
        public DocumentManager(IDocument document)
        {
            Document = document;
        }
    }
}
