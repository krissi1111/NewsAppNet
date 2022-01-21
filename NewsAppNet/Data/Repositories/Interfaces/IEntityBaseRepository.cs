using NewsAppNet.Models.DataModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        int Count();
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetMany(IEnumerable<int> ids);
        T GetSingle(int id);
        T GetSingle(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Commit();
    }
}
