using Microsoft.EntityFrameworkCore;
using System;

namespace Domion.Setup
{
    /// <summary>
    /// Helper to setup the database for a DbContext
    /// </summary>
    /// <typeparam name="T">DbContext</typeparam>
    public abstract class DbSetupHelper<T> where T : DbContext, new()
    {
        private string _connectionString;
        private DbContextOptions<T> _options;

        public DbSetupHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Returns current connection string.
        /// </summary>
        public string ConnectionString => _connectionString;

        /// <summary>
        /// Creates a DbContext if the database hast been set up.
        /// </summary>
        /// <returns>The DbContext with the configured options.</returns>
        public T CreateDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {this.GetType().Name}.{nameof(SetupDatabase)} first!");

            return CreateRawDbContext(_options);
        }

        /// <summary>
        /// Creates an option object for the database with the configured connection string.
        /// </summary>
        /// <returns>The DbContextOptions object to create a DbContext.</returns>
        public virtual DbContextOptions<T> GetOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<T>();

            optionBuilder.UseSqlServer(_connectionString);

            return optionBuilder.Options;
        }

        /// <summary>
        /// Sets up the database with the configured option
        /// </summary>
        public virtual void SetupDatabase()
        {
            MigrateDatabase();
        }

        /// <summary>
        /// Creates a DbContext with the supplied options. Does not check for the database to be created.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected abstract T CreateRawDbContext(DbContextOptions<T> options);

        /// <summary>
        /// Migrates the database to the latest version.
        /// </summary>
        protected virtual void MigrateDatabase()
        {
            lock (_connectionString)
            {
                _options = GetOptions();

                using (var dbContext = CreateDbContext())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
