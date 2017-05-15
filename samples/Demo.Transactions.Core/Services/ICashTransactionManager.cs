//------------------------------------------------------------------------------
// ICashTransactionManager.cs
//
// Implementation of: ICashTransactionManager (Interface) <<entity-manager>>
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
	public interface ICashTransactionManager : IEntityManager<CashTransaction, int>
	{
		CashTransaction Refresh(CashTransaction entity);

		void SaveChanges();

		IEnumerable<ValidationResult> TryDelete(CashTransaction entity);

		IEnumerable<ValidationResult> TryInsert(CashTransaction entity);

		IEnumerable<ValidationResult> TryUpdate(CashTransaction entity);

		IEnumerable<ValidationResult> TryUpsert(CashTransaction entity);

		IEnumerable<ValidationResult> ValidateDelete(CashTransaction entity);

		IEnumerable<ValidationResult> ValidateSave(CashTransaction entity);
	}
}