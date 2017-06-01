//------------------------------------------------------------------------------
// TagConfiguration.cs
//
// Implementation of: TagConfiguration (Class) <<entity-configuration>>
// Generated with Domion-MDA - http://www.coderepo.blog/domion
//------------------------------------------------------------------------------

using Demo.Transactions.Core.Model;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Transactions.Lib.Data
{
    public class TagConfiguration : EntityTypeConfiguration<Tag>
    {
        public override void Map(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags", schema: "Transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.RowVersion)
                .IsRowVersion();
        }
    }
}
