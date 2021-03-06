﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Database.Contracts
{
   public interface IBaseRepository<T> where T: class
    {
        void Add(T item);
        void Remove(T item);
        IEnumerable<T> GetAll();
        IEnumerable<T> Where(Expression<Func<T, bool>> where);
        IEnumerable<T> Include<TKey>(Expression<Func<T, TKey>> expression);

        void SaveChanges();
    }
}
