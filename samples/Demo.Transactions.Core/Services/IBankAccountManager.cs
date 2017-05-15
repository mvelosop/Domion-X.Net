//------------------------------------------------------------------------------
// IBankAccountManager.cs
//
// Implementation of: IBankAccountManager (Interface) <<entity-manager>>
// Generated with Domion-MDA - www.coderepo.blog
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
	public interface IBankAccountManager : IEntityManager<BankAccount, int>
	{
		BankAccount Refresh(BankAccount entity);

		void SaveChanges();

		IEnumerable<ValidationResult> TryDelete(BankAccount entity);

		IEnumerable<ValidationResult> TryInsert(BankAccount entity);

		IEnumerable<ValidationResult> TryUpdate(BankAccount entity);

		IEnumerable<ValidationResult> TryUpsert(BankAccount entity);

		IEnumerable<ValidationResult> ValidateDelete(BankAccount entity);

		IEnumerable<ValidationResult> ValidateSave(BankAccount entity);
	}
}