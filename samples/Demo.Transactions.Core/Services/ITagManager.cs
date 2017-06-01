//------------------------------------------------------------------------------
// ITagManager.cs
//
// Implementation of: ITagManager (Interface) <<entity-manager>>
// Generated with Domion-MDA - http://www.coderepo.blog/domion
//------------------------------------------------------------------------------

using Demo.Transactions.Core.Model;
using Domion.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Transactions.Core.Services
{
    public interface ITagManager : IEntityManager<Tag, int>
    {
        Tag Refresh(Tag entity);

        void SaveChanges();

        IEnumerable<ValidationResult> TryDelete(Tag entity);

        IEnumerable<ValidationResult> TryInsert(Tag entity);

        IEnumerable<ValidationResult> TryUpdate(Tag entity);

        IEnumerable<ValidationResult> TryUpsert(Tag entity);

        IEnumerable<ValidationResult> ValidateDelete(Tag entity);

        IEnumerable<ValidationResult> ValidateSave(Tag entity);
    }
}
