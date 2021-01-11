using System;

namespace CurrentAccount.AnemicDomainModel
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public bool IsOpen { get; set; }

        public decimal Balance { get; set; }

        public decimal OverdraftLimit { get; set; }
    }
}