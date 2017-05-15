//------------------------------------------------------------------------------
// TransactionsDbContext.cs
//
// Implementation of: TransactionsDbContext (Class) <<dbcontext>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using Demo.Budget.Lib.Data;
using Demo.Transactions.Core.Model;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;

namespace Demo.Transactions.Lib.Data
{
	public class TransactionsDbContext : DbContext
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public TransactionsDbContext()
			: base()
		{
		}

		public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options)
			: base(options)
		{
		}

		// Para hacer efectivo el modo del DbContext se debe cambiar el Tag MDA::IsolatedContext
		// en el DbContext del Modelo PIM, ejecutar la Transformación y generar el código nuevamente,
		// porque ese parámetro afecta la generación de las clases del Modelo de Dominio

		public static bool IsolatedContext
		{
			get { return false; }
		}

		public override int SaveChanges()
		{
			try
			{
				return base.SaveChanges();
			}
			catch (Exception ex)
			{
				logger.Error(ex);

				throw;
			}
		}

		/// 
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ConfigureLocalModel(modelBuilder);

			if (!IsolatedContext)
			{
				ConfigureExternalModel(modelBuilder);
			}
		}

		/// 
		/// <param name="modelBuilder"></param>
		private void ConfigureExternalModel(ModelBuilder modelBuilder)
		{
			modelBuilder.AddConfiguration(new BudgetClassConfiguration());
		}

		/// 
		/// <param name="modelBuilder"></param>
		private void ConfigureLocalModel(ModelBuilder modelBuilder)
		{
			// Database schema is "Transactions"

			modelBuilder.AddConfiguration(new BankAccountConfiguration());
			modelBuilder.AddConfiguration(new BankTransactionConfiguration());
			modelBuilder.AddConfiguration(new CashTransactionConfiguration());
			modelBuilder.AddConfiguration(new TagConfiguration());
		}
	}
}