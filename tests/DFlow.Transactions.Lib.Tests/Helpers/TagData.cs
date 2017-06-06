using DFlow.Transactions.Core.Model;

namespace DFlow.Transactions.Lib.Tests.Helpers
{
    public class TagData
    {
        public TagData(string name)
        {
            Name = name;
        }

        public TagData(Tag entity)
        {
            Name = entity.Name;
        }

        public string Name { get; set; }

        public Tag CreateEntity()
        {
            return new Tag
            {
                Name = Name
            };
        }
    }
}
