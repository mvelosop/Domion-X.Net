using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Setup;
using DFlow.Tenants.Core.Model;
using DFlow.Tenants.Lib.Data;
using DFlow.Tenants.Lib.Services;
using Domion.Lib.Extensions;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DFlow.CLI
{
    internal class Program
    {
        private static Tenant _tenant = new Tenant { Owner = "Default Tenant" };

        private static BudgetClass[] _dataSet = new BudgetClass[]
        {
            new BudgetClass { Name = "Income", Order = 1, TransactionType = TransactionType.Income },
            new BudgetClass { Name = "Expenses", Order = 2, TransactionType = TransactionType.Expense },
            new BudgetClass { Name = "Investments", Order = 3, TransactionType = TransactionType.Investment },
        };

        private static BudgetDatabaseHelper _dbHelper;

        private static void LoadSeedData()
        {
            Console.WriteLine("Seeding data...\n");

            Tenant currentTenant = null;

            using (TenantsDbContext dbContext = _dbHelper.TenantsDbHelper.CreateDbContext())
            {
                var repo = new TenantRepository(dbContext);

                currentTenant = repo.SingleOrDefault(t => t.Owner == _tenant.Owner);

                if (currentTenant == null)
                {
                    currentTenant = new Tenant { Owner = _tenant.Owner };

                    repo.TryInsert(currentTenant);

                    repo.SaveChanges();
                }
            }

            using (var dbContext = _dbHelper.CreateDbContext())
            {
                var repo = new BudgetClassRepository(dbContext, currentTenant);

                foreach (var item in _dataSet)
                {
                    var entity = repo.SingleOrDefault(bc => bc.Name.StartsWith(item.Name));

                    if (entity == null)
                    {
                        repo.TryInsert(item);
                    }
                    else
                    {
                        var tokens = entity.Name.Split('-');

                        if (tokens.Length == 1)
                        {
                            entity.Name += " - 1";
                        }
                        else
                        {
                            entity.Name = tokens[0].Trim() + $" - {int.Parse(tokens[1]) + 1}";
                        }
                    }
                }

                repo.SaveChanges();
            }
        }

        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("EF Core App\n");

            ConfigureDb();

            LoadSeedData();

            PrintDb();
        }

        private static void PrintDb()
        {
            using (var dbContext = _dbHelper.CreateDbContext())
            {
                Console.WriteLine("Printing data...\n");

                Console.WriteLine("Budget Classes");
                Console.WriteLine("--------------");

                int nameLength = _dataSet.Select(c => c.Name.Length).Max() + 5;
                int typeLength = _dataSet.Select(c => c.TransactionType.ToString().Length).Max();

                foreach (var item in dbContext.BudgetClasses)
                {
                    Console.WriteLine($"| {item.Name.PadRight(nameLength)} | {item.Order} | {item.TransactionType.ToString().PadRight(typeLength)} |");
                }

                Console.WriteLine();
            }
        }

        private static void ConfigureDb()
        {
            string connectionString = "Data Source=localhost;Initial Catalog=DFlow.CLI;Integrated Security=SSPI;MultipleActiveResultSets=true";

            Console.WriteLine($"Setting up database\n ({connectionString})...\n");

            _dbHelper = new BudgetDatabaseHelper(connectionString);

            _dbHelper.ConfigureDatabase();
        }
    }
}
