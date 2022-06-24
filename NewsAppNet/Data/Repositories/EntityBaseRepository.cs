using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Data.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
        where T : class, IEntityBase, new()
    {
        private readonly DbContext _context;

        public EntityBaseRepository(DbContext context)
        {
            _context = context;
        }
        public async virtual Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }
        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async virtual Task<IEnumerable<T>> GetAllInclude(params Expression<Func<T, IEntityBase>>[] include)
        {
            var set = _context.Set<T>().AsQueryable();
            foreach (var item in include)
            {
                set = set.Include(item);
            }
            return await set.ToListAsync();
        }
        public async virtual Task<IEnumerable<T>> GetAllInclude(params Expression<Func<T, IEnumerable<IEntityBase>>>[] include)
        {
            var set = _context.Set<T>().AsQueryable();
            foreach (var item in include)
            {
                set = set.Include(item);
            }
            return await set.ToListAsync();
        }
        public async virtual Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> query)
        {
            return await _context.Set<T>().Where(query).ToListAsync();
        }
        public async virtual Task<IEnumerable<T>> GetMany(IEnumerable<int> ids)
        {
            return await _context.Set<T>().Where(t => ids.Contains(t.Id)).ToListAsync();
        }
        public async virtual Task<IEnumerable<T>> GetManyInclude(Expression<Func<T, bool>> query, params Expression<Func<T, IEntityBase>>[] include)
        {
            var set = _context.Set<T>().AsQueryable();
            foreach (var item in include)
            {
                set = set.Include(item);
            }
            return await set.Where(query).ToListAsync();
        }
        public async virtual Task<IEnumerable<T>> GetManyIncluded(Expression<Func<T, bool>> query, params Expression<Func<T, IEnumerable<IEntityBase>>>[] include)
        {
            var set = _context.Set<T>().AsQueryable();
            foreach (var item in include)
            {
                set = set.Include(item);
            }
            return await set.Where(query).ToListAsync();
        }
        public async virtual Task<T> GetSingle(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id);
        }
        public async virtual Task<T> GetSingle(Expression<Func<T, bool>> query)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(query);
        }
        /*public async virtual Task<T> GetSingleInclude(Expression<Func<T, bool>> query, params Expression<Func<T, IEntityBase[]>>[] include)
        {

        }*/

        public virtual void Add(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            //_context.Set<T>().Add(entity);
            dbEntityEntry.State = EntityState.Added;// .Add(entity);
        }
        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        // Deletes entity from database if hardDelete is true,
        // otherwise sets IsDeleted variable to true
        public virtual void Delete(T entity, bool hardDelete = false)
        {
            if (!hardDelete)
            {
                entity.IsDeleted = true;
                Update(entity);
            }
            else
            {
                EntityEntry dbEntityEntry = _context.Entry(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
        }
        public virtual void Commit()
        {
            _context.SaveChanges();
        }
    }
}
