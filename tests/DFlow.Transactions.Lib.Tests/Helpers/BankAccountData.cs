using DFlow.Transactions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DFlow.Transactions.Lib.Tests.Helpers
{
    public class BankAccountData
    {
        public BankAccountData()
        {
            Transactions = new List<BankTransactionData>();
        }

        public BankAccountData(string accountName, string accountNumber, string bankName)
            : this()
        {
            AccountName = accountName;

            AccountNumber = accountNumber;
            BankName = bankName;
        }

        public BankAccountData(BankAccount entity)
            : this()
        {
            AccountName = entity.AccountName;

            AccountNumber = entity.AccountNumber;
            BankName = entity.BankName;
        }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal InitialBalance { get; set; }

        public DateTime? InitialBalanceDate { get; set; }

        public Expression<Func<BankAccount, bool>> KeyExpression => t => t.AccountName == AccountName;

        public DateTime? LastTransactionDate { get; set; }

        public List<BankTransactionData> Transactions { get; set; }

        public BankAccount CreateEntity()
        {
            return new BankAccount
            {
                AccountName = AccountName,

                AccountNumber = AccountNumber,
                BankName = BankName
            };
        }
    }
}
