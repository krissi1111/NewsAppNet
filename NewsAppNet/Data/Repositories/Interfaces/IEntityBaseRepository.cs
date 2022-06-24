using NewsAppNet.Models.DataModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        Task<int> Count();
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllInclude(params Expression<Func<T, IEntityBase>>[] include);
        Task<IEnumerable<T>> GetAllInclude(params Expression<Func<T, IEnumerable<IEntityBase>>>[] include);
        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetMany(IEnumerable<int> ids);
        Task<IEnumerable<T>> GetManyInclude(Expression<Func<T, bool>> query, params Expression<Func<T, IEntityBase>>[] include);
        Task<IEnumerable<T>> GetManyIncluded(Expression<Func<T, bool>> query, params Expression<Func<T, IEnumerable<IEntityBase>>>[] include);
        Task<T> GetSingle(int id);
        Task<T> GetSingle(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity, bool hardDelete = false);
        void Commit();
    }
}
