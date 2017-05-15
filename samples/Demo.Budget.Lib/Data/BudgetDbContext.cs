//------------------------------------------------------------------------------
// BudgetDbContext.cs
//
// Implementation of: BudgetDbContext (Class) <<dbcontext>>
// Generated with Domion-MDA - www.coderepo.blog
//------------------------------------------------------------------------------

using Demo.Budget.Core.Model;
using Domion.Lib.Data;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;

namespace Demo.Budget.Lib.Data
{
	public class BudgetDbContext : DbContext
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public BudgetDbContext()
			: base()
		{
		}

		public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
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
		}

		/// 
		/// <param name="modelBuilder"></param>
		private void ConfigureLocalModel(ModelBuilder modelBuilder)
		{
			// Database schema is "Budget"

			modelBuilder.AddConfiguration(new BudgetClassConfiguration());
		}
	}
}