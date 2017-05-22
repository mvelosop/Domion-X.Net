using Demo.Transactions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Demo.Transactions.Lib.Tests.Helpers
{
    public class BankAccountData
    {
        public BankAccountData()
        {
            Transactions = new List<BankTransactionData>();
        }

        public BankAccountData(string accountNumber, string bankName)
            : this()
        {
            AccountNumber = accountNumber;
            BankName = bankName;
        }

        public BankAccountData(BankAccount entity)
            : this()
        {
            AccountNumber = entity.AccountNumber;
            BankName = entity.BankName;
        }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal InitialBalance { get; set; }

        public DateTime? InitialBalanceDate { get; set; }

        public Expression<Func<BankAccount, bool>> KeyExpression => t => t.AccountNumber == AccountNumber;

        public DateTime? LastTransactionDate { get; set; }

        public List<BankTransactionData> Transactions { get; set; }

        public BankAccount CreateEntity()
        {
            return new BankAccount
            {
                AccountNumber = AccountNumber,
                BankName = BankName
            };
        }
    }
}
