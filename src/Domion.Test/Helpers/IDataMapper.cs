namespace Domion.Test.Helpers
{
    public interface IDataMapper<TData, TEntity>
    {
        TData CreateData(TEntity entity);

        TEntity CreateEntity(TData data);

        TEntity UpdateEntity(TEntity entity, TData data);
    }
}
