using DFlow.Transactions.Core.Model;
using System;
using System.Linq.Expressions;

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

        public Expression<Func<Tag, bool>> KeyExpression => t => t.Name == Name;

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
