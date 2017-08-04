using DFlow.Budget.Core.Model;
using Domion.DataTools;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public class BudgetClassDataMapper : IDataMapper<BudgetClassData, BudgetClass>
    {
        public BudgetClassData CreateData(BudgetClass entity)
        {
            var data = new BudgetClassData(entity.Name, entity.TransactionType);

            return data;
        }

        public BudgetClass CreateEntity(BudgetClassData data)
        {
            var entity = new BudgetClass();

            return UpdateEntity(data, entity);
        }

        public BudgetClass UpdateEntity(BudgetClassData data, BudgetClass entity)
        {
            entity.Name = data.Name;
            entity.TransactionType = data.TransactionType;

            return entity;
        }
    }
}
