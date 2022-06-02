using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Data.Interfaces;
using StudyControlWeb.Models;
using StudyControlWeb.Models.DBO;

namespace StudyControlWeb.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private DataContext _context;
        public Repository(DataContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            if (entity != null)
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            var entity = _context.Set<T>().FirstOrDefault(x => x.Id.ToString() == id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }

        public T Get(string id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id.ToString() == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public void Update(T entity)
        {
            if (entity != null)
            {
                if (_context.Faculty.Any(x => x.Id == entity.Id))
                {
                    _context.Set<T>().Update(entity);
                    _context.SaveChanges();
                }
            }
        }
    }
}
