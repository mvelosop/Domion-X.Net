//------------------------------------------------------------------------------
// BudgetClassConfiguration.cs
//
// Implementation of: BudgetClassConfiguration (Class) <<entity-configuration>>
// Generated with Domion-MDA - http://www.coderepo.blog/domion
//------------------------------------------------------------------------------

using Demo.Budget.Core.Model;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Budget.Lib.Data
{
    public class BudgetClassConfiguration : EntityTypeConfiguration<BudgetClass>
    {
        public override void Map(EntityTypeBuilder<BudgetClass> builder)
        {
            builder.ToTable("BudgetClasses", schema: "Budget");

            builder.HasKey(bc => bc.Id);

            builder.Property(bc => bc.RowVersion)
                .IsRowVersion();

            builder.HasIndex(bc => bc.Name)
                .IsUnique();
        }
    }
}
