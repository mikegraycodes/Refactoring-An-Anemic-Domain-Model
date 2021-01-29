using System;

namespace BankAccounts.Rich
{
    public class BankAccount
    {
        public Guid Id { get; }
        public bool IsOpen { get; private set; }
        public decimal Balance { get; private set; }
        private decimal overdraftLimit;
        public decimal OverdraftLimit => overdraftLimit * -1;

        private BankAccount()
        {
            Id = Guid.NewGuid();
            IsOpen = true;
            Balance = 0;
            overdraftLimit = 0;
        }

        public void DepositFunds(decimal amount)
        {
            CheckAccountNotClosed();
            if (amount <= 0)
                throw new InvalidOperationException("Amount deposited must be greater than 0");
            Balance += amount;
        }

        public void WithdrawFunds(decimal amount)
        {
            CheckAccountNotClosed();
            
            if (Balance - amount < overdraftLimit)
                throw new InvalidOperationException("Amount withdrawn must be greater than 0");
            
            if (Balance - amount < overdraftLimit)
                throw new InvalidOperationException("Cannot withdraw funds that would put balance below overdraft");

            Balance -= amount;
        }

        public void UpdateOverdraftLimit(decimal overdraftLimit)
        {
            CheckAccountNotClosed();
            if(Balance < overdraftLimit * -1) 
                throw new InvalidOperationException("Overdraft limit cannot be lower than current account balance");
            if(overdraftLimit < 0) 
                throw new InvalidOperationException("Overdraft limit cannot be less than 0");

            this.overdraftLimit = overdraftLimit * -1;
        }

        public void Close()
        {
            if (Balance < 0)
                throw new InvalidOperationException(("Account cannot be closed if balance is negative"));

            IsOpen = false;
        }
        
        public void ReOpen()
        {
            if(IsOpen) return;
            
            IsOpen = true;
        }

        private void CheckAccountNotClosed()
        {
            if(!IsOpen)
                throw new InvalidOperationException("Cannot modify closed account");
        }
        
        
        public static BankAccount Open()
        {
            return new BankAccount();
        }
    }
}
