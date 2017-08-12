namespace Domion.DataHelpers
{
    public interface IDataMapper<TData, TEntity> where TData : class where TEntity : class
    {
        TData CreateData(TEntity entity);

        TEntity CreateEntity(TData data);

        TEntity UpdateEntity(TData data, TEntity entity);
    }
}
