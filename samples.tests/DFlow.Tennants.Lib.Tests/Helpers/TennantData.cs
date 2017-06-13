using DFlow.Tennants.Core.Model;

namespace DFlow.Tennants.Lib.Tests.Helpers
{
    public class TennantData
    {
        public TennantData()
        {
        }

        public TennantData(string name)
        {
            Owner = name;
        }

        public TennantData(Tennant entity)
        {
            Owner = entity.Owner;
        }

        public string Owner { get; set; }

        public Tennant CreateEntity()
        {
            return UpdateEntity(new Tennant());
        }

        public Tennant UpdateEntity(Tennant entity)
        {
            entity.Owner = Owner;

            return entity;
        }
    }
}
