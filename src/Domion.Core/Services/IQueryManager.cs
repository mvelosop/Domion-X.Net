using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Core.Services
{
    public interface IQueryManager<T> where T : class
    {
        IQueryable<T> Query();

        IQueryable<T> Query(Expression<Func<T, bool>> where);
    }
}
