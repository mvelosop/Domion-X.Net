using Domion.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Lib.Data
{
    public class BaseRepository<T, TKey> : IEntityManager<T, TKey> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        protected virtual DbContext DbContext => _dbContext;

        public void Detach(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        public virtual T Find(TKey key)
        {
            return _dbContext.Find<T>(key);
        }

        public virtual T First(Expression<Func<T, bool>> where)
        {
            return Query(where).First();
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> where)
        {
            return Query(where).FirstOrDefault<T>();
        }

        public virtual T GetOriginalEntity(T entity)
        {
            var entry = DbContext.Entry(entity);

            if (entry == null)
            {
                return null;
            }

            return entry.OriginalValues.ToObject() as T;
        }

        public virtual IQueryable<T> Query()
        {
            return Query(null);
        }

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            IQueryable<T> query = _dbSet;

            if (where != null)
            {
                query = query.Where(where);
            }

            return query;
        }

        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public virtual T Single(Expression<Func<T, bool>> where)
        {
            return Query(where).Single<T>();
        }

        public virtual T SingleOrDefault(Expression<Func<T, bool>> where)
        {
            return Query(where).SingleOrDefault<T>();
        }

        protected virtual IEnumerable<ValidationResult> TryDelete(T entity)
        {
            var deleteErrors = ValidateDelete(entity);

            if (deleteErrors.Any())
            {
                return deleteErrors;
            }

            _dbSet.Remove(entity);

            return Enumerable.Empty<ValidationResult>();
        }

        protected virtual IEnumerable<ValidationResult> TryInsert(T entity)
        {
            var saveErrors = ValidateSave(entity);

            if (saveErrors.Any())
            {
                return saveErrors;
            }

            _dbSet.Add(entity);

            return Enumerable.Empty<ValidationResult>();
        }

        protected virtual IEnumerable<ValidationResult> TryUpdate(T entity)
        {
            var saveErrors = ValidateSave(entity);

            if (saveErrors.Any())
            {
                return saveErrors;
            }

            _dbSet.Update(entity);

            return Enumerable.Empty<ValidationResult>();
        }

        protected virtual IEnumerable<ValidationResult> ValidateDelete(T model)
        {
            yield break;
        }

        protected virtual IEnumerable<ValidationResult> ValidateSave(T model)
        {
            yield break;
        }
    }
}
