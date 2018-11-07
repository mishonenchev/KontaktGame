using KontaktGame.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Database.Repositories
{
    public abstract class BaseRepository<T>: IBaseRepository<T> where T: class
    {
        private readonly DbSet<T> _dbSet;
        private readonly IDatabase database;
        public BaseRepository(IDatabase database)
        {
            _dbSet = database.Set<T>();
            this.database = database;
        }
        public void Add(T item)
        {
            _dbSet.Add(item);
        }
        public virtual void Remove(T item)
        {
            _dbSet.Remove(item);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }
        public virtual IEnumerable<T> Where(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where);
        }
        public virtual IEnumerable<T> Include<TKey>(Expression<Func<T, TKey>> expression)
        {
            return _dbSet.Include<T, TKey>(expression).ToList();
        }

        public virtual void SaveChanges()
        {
            database.SaveChanges();
        }
    }
}
