using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Core.Services
{
    public interface IEntityManager<T, TKey> where T : class
    {
        T Find(TKey key);
    }

    public interface IEntityManager<T> where T : class
    {
        T First(Expression<Func<T, bool>> where);

        T FirstOrDefault(Expression<Func<T, bool>> where);

        IQueryable<T> Query();

        IQueryable<T> Query(Expression<Func<T, bool>> where);

        T Single(Expression<Func<T, bool>> where);

        T SingleOrDefault(Expression<Func<T, bool>> where);
    }
}
