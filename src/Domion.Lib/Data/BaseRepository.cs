using Domion.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domion.Lib.Data
{
    /// <summary>
    ///     Generic repository implementation.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Key property type</typeparam>
    public abstract class BaseRepository<TEntity, TKey> : IRepositoryQuery<TEntity>, IEntityFinder<TEntity, TKey> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        ///     Creates the generic repository instance.
        /// </summary>
        /// <param name="dbContext">The DbContext to get the Entity Type from.</param>
        protected BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        protected virtual DbContext DbContext => _dbContext;

        /// <summary>
        ///     Detaches the entity from the DbContext's change tracker.
        /// </summary>
        public void Detach(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        /// <inheritdoc />
        public virtual TEntity Find(TKey key)
        {
            return FindAsync(key).Result;
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> FindAsync(TKey key)
        {
            return await _dbContext.FindAsync<TEntity>(key);
        }

        /// <summary>
        ///     Returns an entity object with the original values when it was last read from the database.
        ///     Does not include any navigation properties, not even collections.
        /// </summary>
        public virtual TEntity GetOriginalEntity(TEntity entity)
        {
            var entry = DbContext.Entry(entity);

            return entry?.OriginalValues.ToObject() as TEntity;
        }

        /// <inheritdoc />
        public virtual IQueryable<TEntity> Query()
        {
            return Query(null);
        }

        /// <inheritdoc />
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
            {
                query = query.Where(where);
            }

            return query;
        }

        /// <summary>
        ///     Saves changes from the DbContext's change tracker to the database.
        /// </summary>
        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        ///     Saves changes from the DbContext's change tracker to the database.
        /// </summary>
        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Validates if it's ok to delete the entity from the database.
        /// </summary>
        public virtual IEnumerable<ValidationResult> ValidateDelete(TEntity model)
        {
            yield break;
        }

        /// <summary>
        ///     Validates if it's ok to save the new or updated entity to the database.
        /// </summary>
        public virtual IEnumerable<ValidationResult> ValidateSave(TEntity model)
        {
            yield break;
        }

        /// <summary>
        ///     Marks an entity for deletion in the DbContext's change tracker if it passes the ValidateDelete method.
        /// </summary>
        protected virtual List<ValidationResult> TryDelete(TEntity entity)
        {
            List<ValidationResult> errors = ValidateDelete(entity).ToList();

            if (errors.Count > 0)
            {
                return errors;
            }

            _dbSet.Remove(entity);

            return errors;
        }

        /// <summary>
        ///     Adds an entity for insertion in the DbContext's change tracker if it passes the ValidateSave method.
        /// </summary>
        protected virtual List<ValidationResult> TryInsert(TEntity entity)
        {
            List<ValidationResult> errors = ValidateSave(entity).ToList();

            if (errors.Count > 0)
            {
                return errors;
            }

            _dbSet.Add(entity);

            return errors;
        }

        /// <summary>
        ///     Marks an entity for update in the DbContext's change tracker if it passes the ValidateSave method.
        /// </summary>
        protected virtual List<ValidationResult> TryUpdate(TEntity entity)
        {
            List<ValidationResult> errors = ValidateSave(entity).ToList();

            if (errors.Count > 0)
            {
                return errors;
            }

            _dbSet.Update(entity);

            return errors;
        }
    }
}
