using Demo.Transactions.Core.Model;
using Demo.Transactions.Lib.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Demo.Transactions.Lib.Tests.Helpers
{
    public class BankAccountHelper
    {
        public BankAccountHelper(TransactionsDbSetup dbSetup)
        {
            DbSetup = dbSetup;
        }

        public TransactionsDbSetup DbSetup { get; set; }

        public void AssertDelete(BankAccount entity)
        {
            var errors = TryDelete(entity);

            errors.Should().BeEmpty();
        }

        public void AssertEntitiesDoNotExist(params BankAccountData[] dataArray)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

                foreach (var data in dataArray)
                {
                    var entities = manager.Query(ci => ci.AccountNumber == data.AccountNumber).ToList();

                    foreach (var entity in entities)
                    {
                        var errors = manager.TryDelete(entity);

                        errors.Should().BeEmpty();
                    }
                }

                manager.SaveChanges();
            }

            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

                foreach (var data in dataArray)
                {
                    var entities = manager.Query(ci => ci.AccountNumber == data.AccountNumber).ToList();

                    entities.Should().BeEmpty();
                }
            }
        }

        public void AssertInsert(BankAccount entity)
        {
            var errors = TryInsert(entity);

            errors.Should().BeEmpty();
        }

        public void AssertUpdate(BankAccount entity)
        {
            var errors = TryUpdate(entity);

            errors.Should().BeEmpty();
        }

        public BankAccount GetEntity(BankAccountData data)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

                return manager.SingleOrDefault(ci => ci.AccountNumber == data.AccountNumber);
            }
        }

        public IEnumerable<ValidationResult> TryDelete(BankAccount entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

                var errors = manager.TryDelete(entity);

                if (!errors.Any())
                {
                    manager.SaveChanges();
                }

                return errors.ToList();
            }
        }

        public IEnumerable<ValidationResult> TryInsert(BankAccount entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

                var errors = manager.TryInsert(entity);

                if (!errors.Any())
                {
                    manager.SaveChanges();
                }

                return errors.ToList();
            }
        }

        public IEnumerable<ValidationResult> TryUpdate(BankAccount entity)
        {
            using (var dbContext = DbSetup.GetDbContext())
            {
                var manager = new BankAccountManager(dbContext);

                var errors = manager.TryUpdate(entity);

                if (!errors.Any())
                {
                    manager.SaveChanges();
                }

                return errors.ToList();
            }
        }

    }
}
