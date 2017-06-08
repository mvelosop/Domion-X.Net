using Autofac;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Budget.Setup;
using DFlow.Tennants.Core.Model;
using DFlow.Tennants.Lib.Services;
using DFlow.Tennants.Lib.Tests.Helpers;
using DFlow.Tennants.Setup;
using Domion.FluentAssertions.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DFlow.Budget.Lib.Tests
{
    [Trait("Type", "Integration")]
    public class BudgetClassManager_IntegrationTests
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private IContainer _container;

        public BudgetClassManager_IntegrationTests()
        {
            BudgetDbSetupHelper dbHelper = SetupDatabase(_connectionString);

            _container = SetupContainer(dbHelper);

            SetupReferenceData();
        }

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var tennantOwner = "Reference Tennant #1";
            var data = new BudgetClassData("Delete-Success-Valid - Inserted", "Income");

            ExecuteInLocalScope<BudgetClassManagerHelper>(tennantOwner, managerHelper =>
            {
                managerHelper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            ExecuteInLocalScope<BudgetClassManager>(tennantOwner, manager =>
            {
                var entity = manager.AssertGetByKeyData(data.Name);

                errors = manager.TryDelete(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            ExecuteInLocalScope<BudgetClassManagerHelper>(tennantOwner, managerHelper =>
            {
                managerHelper.EnsureEntitiesDoNotExist(data);
            });
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var tennantOwner = "Reference Tennant #1";
            var data = new BudgetClassData("Insert-Error-Duplicate - Inserted", "Income");

            ExecuteInLocalScope<BudgetClassManagerHelper>(tennantOwner, managerHelper =>
            {
                managerHelper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            ExecuteInLocalScope<BudgetClassManager>(tennantOwner, manager =>
            {
                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var tennantOwner = "Reference Tennant #1";
            var data = new BudgetClassData("Insert-Success-Valid - Inserted", "Income");

            ExecuteInLocalScope<BudgetClassManagerHelper>(tennantOwner, managerHelper =>
            {
                managerHelper.EnsureEntitiesDoNotExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            ExecuteInLocalScope<BudgetClassManager>(tennantOwner, manager =>
            {
                var entity = data.CreateEntity();

                errors = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            ExecuteInLocalScope<BudgetClassManagerHelper>("Reference Tennant #1", managerHelper =>
            {
                managerHelper.AssertEntitiesExist(data);
            });
        }

        [Fact]
        public void TryInsert_InsertsRecords_WhenDuplicateKeyDataButDifferentTennants()
        {
            // Arrange ---------------------------

            var firstTennantOwner = "Reference Tennant #1";
            var secondTennantOwner = "Reference Tennant #2";

            var data = new BudgetClassData("Insert-Success-Duplicate-Different-Tennant - Inserted", "Income");

            ExecuteInLocalScope<BudgetClassManagerHelper>(firstTennantOwner, managerHelper =>
            {
                managerHelper.EnsureEntitiesDoNotExist(data);
            });

            ExecuteInLocalScope<BudgetClassManagerHelper>(secondTennantOwner, managerHelper =>
            {
                managerHelper.EnsureEntitiesDoNotExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errorsFirst = null;
            IEnumerable<ValidationResult> errorsSecond = null;

            ExecuteInLocalScope<BudgetClassManager>(firstTennantOwner, manager =>
            {
                var entity = data.CreateEntity();

                errorsFirst = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            ExecuteInLocalScope<BudgetClassManager>(secondTennantOwner, manager =>
            {
                var entity = data.CreateEntity();

                errorsSecond = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errorsFirst.Should().BeEmpty();
            errorsSecond.Should().BeEmpty();
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var tennantOwner = "Reference Tennant #1";
            var dataFirst = new BudgetClassData("Update-Error-Duplicate - Inserted first", "Income");
            var dataSecond = new BudgetClassData("Update-Error-Duplicate - Inserted second", "Income");

            ExecuteInLocalScope<BudgetClassManagerHelper>(tennantOwner, managerHelper =>
            {
                managerHelper.EnsureEntitiesExist(dataFirst, dataSecond);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            ExecuteInLocalScope<BudgetClassManager>(tennantOwner, manager =>
            {
                var entity = manager.AssertGetByKeyData(dataFirst.Name);

                entity.Name = dataSecond.Name;

                errors = manager.TryUpdate(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var tennantOwner = "Reference Tennant #1";
            var data = new BudgetClassData("Update-Success-Valid - Inserted", "Income");
            var update = new BudgetClassData("Update-Success-Valid - Updated", "Income");

            ExecuteInLocalScope<BudgetClassManagerHelper>(tennantOwner, managerHelper =>
            {
                managerHelper.EnsureEntitiesExist(data);
                managerHelper.EnsureEntitiesDoNotExist(update);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            ExecuteInLocalScope<BudgetClassManager>(tennantOwner, manager =>
            {
                var entity = manager.AssertGetByKeyData(data.Name);

                entity.Name = update.Name;

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            ExecuteInLocalScope<BudgetClassManagerHelper>(tennantOwner, managerHelper =>
            {
                managerHelper.EnsureEntitiesExist(update);
            });
        }

        /// <summary>
        /// Creates a local context injecting the Tennant object found by "owner" param, resolves the agent and invokes the action passing the agent
        /// </summary>
        /// <typeparam name="T">Agent type</typeparam>
        /// <param name="owner">Tennant key data</param>
        /// <param name="action">Action to execute</param>
        private void ExecuteInLocalScope<T>(string owner, Action<T> action)
        {
            Tennant tennant = GetTennant(owner);

            using (var scope = GetLocalScope(cb => cb.Register(c => tennant)))
            {
                var agent = scope.Resolve<T>();

                action.Invoke(agent);
            }
        }

        /// <summary>
        /// Creates a local context injecting the Tennant object found by "owner" param and invokes the action passing the context
        /// </summary>
        /// <param name="owner">Tennant key data</param>
        /// <param name="action">Action to execute</param>
        private void ExecuteInLocalScope(string owner, Action<ILifetimeScope> action)
        {
            Tennant tennant = GetTennant(owner);

            using (var scope = GetLocalScope(cb => cb.Register(c => tennant)))
            {
                action.Invoke(scope);
            }
        }

        private ILifetimeScope GetLocalScope(Action<ContainerBuilder> scopeContext, IContainer scope = null)
        {
            IContainer container = scope ?? _container;

            if (scopeContext == null)
            {
                return container.BeginLifetimeScope();
            }
            else
            {
                return container.BeginLifetimeScope(scopeContext);
            }
        }

        private Tennant GetTennant(string owner)
        {
            Tennant tennant = null;

            using (var scope = GetLocalScope(null))
            {
                var manager = scope.Resolve<TennantManager>();

                tennant = manager.AssertGetByKeyData(owner);
            }

            return tennant;
        }

        private Action<ContainerBuilder> GetTennantContext(string owner)
        {
            Action<ContainerBuilder> scopeContext = cb =>
            {
                cb.Register((c) => GetTennant(owner));
            };

            return scopeContext;
        }

        private IContainer SetupContainer(BudgetDbSetupHelper dbHelper)
        {
            var budgetDiHelper = new BudgetAutofacSetupHelper(dbHelper);

            var builder = new ContainerBuilder();

            budgetDiHelper.SetupContainer(builder);

            // Tennants --------------------------

            TennantsDbSetupHelper tennantsDbHelper = new TennantsDbSetupHelper(_connectionString);

            tennantsDbHelper.SetupDatabase();

            var tennantsDiHelper = new TennantsAutofacSetupHelper(tennantsDbHelper);

            tennantsDiHelper.SetupContainer(builder);

            //------------------------------------

            IContainer container = builder.Build();

            return container;
        }

        private BudgetDbSetupHelper SetupDatabase(string connectionString)
        {
            BudgetDbSetupHelper dbHelper = new BudgetDbSetupHelper(_connectionString);

            //using (var dbContext = new BudgetDbContext(dbHelper.GetOptions()))
            //{
            //    var serviceProvider = dbContext.GetInfrastructure<IServiceProvider>();
            //    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            //    loggerFactory.AddProvider(new MyLoggerProvider());

            //}

            dbHelper.SetupDatabase();

            return dbHelper;
        }

        private void SetupReferenceData()
        {
            var tennantsDataHelper = new TennantsTestDataHelper(_container);

            tennantsDataHelper.SeedReferenceTennants();
        }
    }

    //public class MyLoggerProvider : ILoggerProvider
    //{
    //    public ILogger CreateLogger(string categoryName)
    //    {
    //        return new MyLogger();
    //    }

    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private class MyLogger : ILogger
    //    {
    //        public IDisposable BeginScope<TState>(TState state)
    //        {
    //            return null;
    //        }

    //        public bool IsEnabled(LogLevel logLevel)
    //        {
    //            return true;
    //        }

    //        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    //        {
    //            Debug.WriteLine(formatter(state, exception));
    //        }
    //    }
    //}
}
