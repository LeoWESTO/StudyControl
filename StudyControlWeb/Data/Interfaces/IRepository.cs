namespace StudyControlWeb.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(string id);
        IEnumerable<T> GetAll();
        void Add (T entity);
        void Update (T entity);
        void Delete (string id);
    }
}
