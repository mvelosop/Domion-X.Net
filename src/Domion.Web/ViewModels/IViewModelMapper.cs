namespace Domion.Web.ViewModels
{
    public interface IViewModelMapper<TViewModel, TEntity> where TViewModel : class where TEntity : class
    {
        TEntity CreateEntity(TViewModel vm);

        TViewModel CreateViewModel(TEntity entity);

        TEntity UpdateEntity(TViewModel vm, TEntity entity);
    }
}
