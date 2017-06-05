using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Core.Services
{
    public interface IEntityManager<T, TKey> where T : class
    {
        T Find(TKey key);
    }

}
