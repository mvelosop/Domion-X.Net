namespace Domion.WebApp.ViewModels
{
    public interface IViewModelMapper<TViewModel, TEntity> where TViewModel : class where TEntity : class
    {
        TEntity CreateEntity(TViewModel vm);

        TViewModel CreateViewModel(TEntity entity);

        TEntity UpdateEntity(TEntity entity, TViewModel vm);
    }
}
