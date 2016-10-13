using LearningManagementSystem.Interfacelasses;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LearningManagementSystem.Models;
using PagedList;

namespace LearningManagementSystem.Data
{
    public class Repository<T> :IRepository<T> where T : class
    {

        private LMSContext db;
        private DbSet<T> dbSet;


        public Repository()
        {
            db = new LMSContext();
            dbSet = db.Set<T>();
        }
        public async Task Delete(object Id)
        {
            T getObjById = await dbSet.FindAsync(Id);
            dbSet.Remove(getObjById);
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetById(object Id)
        {
            return await dbSet.FindAsync(Id);
        }

        public void Insert(T obj)
        {
            dbSet.Add(obj);
        }

        public  void Save()
        {
            db.SaveChanges();
        }

        public void Update(T obj)
        {
            db.Entry(obj).State = EntityState.Modified;
        }
        private bool _disposed = false;
        public void Dispose()
        {
            if (!_disposed)
            {
                db.Dispose();
            }

            _disposed = true;
        }

        public IPagedList<Batch> GetAllCourses(int? page)
        {
            return  db.batch.OrderByDescending(or => or.BatchID).ToList().ToPagedList(page ?? 1, 4);
        }
    }
}