using System;
using FluentAssertions;
using Xunit;

namespace BankAccounts.Rich.Tests
{
    public class BankAccountTests
    {
        [Fact]
        public void Open_CreatesBankAccount_WithValidState()
        {
            // Arrange
            // Act
            var bankAccount = BankAccount.Open();

            // Assert
            bankAccount.Balance.Should().Be(0);
            bankAccount.Id.Should().NotBe(Guid.Empty);
            bankAccount.IsOpen.Should().BeTrue();
            bankAccount.OverdraftLimit.Should().Be(0);
        }
        
        [Fact]
        public void DepositFunds_WhenDepositAmountIsZero_ShouldFail()
        {
            // Arrange
            var bankAccount = BankAccount.Open();

            // Act
            Action act = () => bankAccount.DepositFunds(0);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void DepositFunds_WhenDepositAmountIsNegative_ShouldFail()
        {
            // Arrange
            var bankAccount = BankAccount.Open();

            // Act
            Action act = () => bankAccount.DepositFunds(-5);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [InlineData(1)]
        [InlineData(100)]
        [InlineData(1000000)]
        [InlineData(999999999)]
        [Theory]
        public void DepositFunds_WhenValidAmountIsAdded_AddsFundsToBankAccount(decimal amount)
        {
            // Arrange
            var bankAccount = BankAccount.Open();

            // Act
            bankAccount.DepositFunds(amount);

            // Assert
            bankAccount.Balance.Should().Be(amount);
        }


        [InlineData(1)]
        [InlineData((50))]
        [InlineData(5000000000.23)]
        [Theory]
        public void DepositFunds_WhenPositiveAmount_BalanceShouldBeCorrect(decimal amount)
        {
            //Arrange
            var bankAccount = BankAccount.Open();

            //Act
            bankAccount.DepositFunds(amount);
            
            
            //Assert
            bankAccount.Balance.Should().Be(amount);
        }
        
        [InlineData(-1)]
        [InlineData(-50)]
        [InlineData(-100)]
        [Theory]
        public void DepositFunds_NegativeAmount_BalanceShouldThrowInvalidOperationException(decimal amount)
        {
            //Arrange
            var bankAccount = BankAccount.Open();
            bankAccount.DepositFunds(5000);

            //Act
            Action act = () => bankAccount.DepositFunds(amount);
            
            
            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Amount deposited must be greater than 0");
        }
        
        [Fact]
        public void DepositFunds_WhenAccountIsClosed_ShouldFail()
        {
            //Arrange
            var bankAccount = BankAccount.Open();
            bankAccount.Close();

            //Act
            Action act = () => bankAccount.DepositFunds(100);
            
            //Assert
            act.Should().Throw<InvalidOperationException>();
        }


        [InlineData(-1)]
        [InlineData(-50)]
        [InlineData(-100)]
        [Theory]
        public void WithdrawFunds_NegativeAmount_ShouldFail(decimal negativeAmount)
        {
            //Arrange
            var bankAccount = BankAccount.Open();

            //Act
            Action act = () => bankAccount.WithdrawFunds(negativeAmount);

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Amount withdrawn must be greater than 0");
        }


        [Fact]
        public void WithdrawFunds_WithNoBalanceAvailable_ShouldFail()
        {
            //Arrange
            var bankAccount = BankAccount.Open();

            //Act
            Action act = () => bankAccount.WithdrawFunds(100);

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot withdraw funds that would put balance below overdraft");
        }


        [Fact]
        public void WithdrawFunds_WhenAccountIsClosed_ShouldFail()
        {
            //Arrange
            var bankAccount = BankAccount.Open();
            bankAccount.Close();

            //Act
            Action act = () => bankAccount.WithdrawFunds(100);

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot modify closed account");
        }

        [Fact]
        public void UpdateOverdraftLimit_WhenLessThanBalance_ShouldFail()
        {
            //Arrange
            var bankAccount = BankAccount.Open();
            bankAccount.UpdateOverdraftLimit(100);
            bankAccount.WithdrawFunds(50);

            //Act
            Action act = () => bankAccount.UpdateOverdraftLimit(0);

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Overdraft limit cannot be lower than current account balance");
        }

        [Fact]
        public void UpdateOverdraftLimit_WhenNegativeUpdate_ShouldFail()
        {
            //Arrange
            var bankAccount = BankAccount.Open();

            //Act
            Action act = () => bankAccount.UpdateOverdraftLimit(-100);

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Overdraft limit cannot be less than 0");
        }

        [Fact]
        public void UpdateOverdraftLimit_WhenAccountIsClosed_ShouldFail()
        {
            //Arrange
            var bankAccount = BankAccount.Open();
            bankAccount.Close();

            //Act
            Action act = () => bankAccount.UpdateOverdraftLimit(100);

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot modify closed account");
        }

        [Fact]
        public void UpdateOverdraftLimit_WhenValid_ShouldSucceed()
        {
            //Arrange
            var bankAccount = BankAccount.Open();

            //Act
            bankAccount.UpdateOverdraftLimit(100);

            //Assert
            bankAccount.OverdraftLimit.Should().Be(100);
        }


        [Fact]
        public void Close_WhenAccountIsOpenAndBalanceIs0_ShouldSucceed()
        {
            //Arrange
            var bankAccount = BankAccount.Open();

            //Act
            bankAccount.Close();

            //Assert
            bankAccount.IsOpen.Should().BeFalse();
        }

        [Fact]
        public void Close_WhenBalanceIsNegative_ShouldFail()
        {
            //Arrange
            var bankAccount = BankAccount.Open();
            bankAccount.UpdateOverdraftLimit(100);
            bankAccount.WithdrawFunds(50);

            //Act
            Action act = () => bankAccount.Close();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Account cannot be closed if balance is negative");
        }

        [Fact]
        public void ReOpen_WhenAccountIsOpen_ShouldFail()
        {
            //Arrange
            var bankAccount = BankAccount.Open();

            //Act
            Action act = () => bankAccount.ReOpen();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Account is already open");
        }

        [Fact]
        public void ReOpen_WhenAccountIsClosed_ShouldSucceed()
        {
            //Arrange
            var bankAccount = BankAccount.Open();
            bankAccount.Close();

            //Act
            bankAccount.ReOpen();

            //Assert
            bankAccount.IsOpen.Should().BeTrue();
        }

    }
}