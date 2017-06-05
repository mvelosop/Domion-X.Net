namespace Domion.Core.Services
{
    public interface IEntityManager<T, TKey> where T : class
    {
        T Find(TKey key);
    }
}
