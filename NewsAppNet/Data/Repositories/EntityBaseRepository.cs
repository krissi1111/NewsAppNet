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
        private DbContext _context;

        public EntityBaseRepository(DbContext context)
        {
            _context = context;
        }
        public virtual int Count()
        {
            return _context.Set<T>().Count();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> query)
        {
            return _context.Set<T>().Where(query);
        }
        public virtual IEnumerable<T> GetMany(IEnumerable<int> ids)
        {
            return _context.Set<T>().Where(t => ids.Contains(t.Id));
        }
        public virtual T GetSingle(int id)
        {
            return _context.Set<T>().FirstOrDefault(entity => entity.Id == id);
        }
        public virtual T GetSingle(Expression<Func<T, bool>> query)
        {
            return _context.Set<T>().FirstOrDefault(query);
        }
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
