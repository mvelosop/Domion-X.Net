using Demo.Transactions.Core.Model;
using System;

namespace Demo.Transactions.Lib.Tests.Helpers
{
    public class BankTransactionData
    {
        public BankTransactionData(DateTime date, DateTime valueDate, decimal amount, string currency, string description, string reference, string type)
        {
            Date = date;
            ValueDate = valueDate;
            Amount = amount;
            Currency = currency;
            Description = description;
            Reference = reference;
            Type = type;
        }

        public BankTransactionData(BankTransaction entity)
        {
            Amount = entity.Amount;
            BankNotes = entity.BankNotes;
            Currency = entity.Currency;
            Date = entity.Date;
            Description = entity.Description;
            IsCashWithdrawal = entity.IsCashWithdrawal;
            Notes = entity.Notes;
            Reference = entity.Reference;
            Type = entity.Type;
            ValueDate = entity.ValueDate;
        }

        public decimal Amount { get; set; }

        public string BankNotes { get; set; }

        public string Currency { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public bool IsCashWithdrawal { get; set; }

        public string Notes { get; set; }

        public string Reference { get; set; }

        public string Type { get; set; }

        public DateTime ValueDate { get; set; }

        public BankTransaction CreateEntity()
        {
            return new BankTransaction
            {
                Amount = Amount,
                BankNotes = BankNotes,
                Currency = Currency,
                Date = Date,
                Description = Description,
                IsCashWithdrawal = IsCashWithdrawal,
                Notes = Notes,
                Reference = Reference,
                Type = Type,
                ValueDate = ValueDate,
            };
        }
    }
}
