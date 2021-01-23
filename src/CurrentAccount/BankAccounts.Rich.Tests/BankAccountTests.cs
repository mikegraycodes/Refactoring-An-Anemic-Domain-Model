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

            //Act
            Action act = () => bankAccount.DepositFunds(amount);
            
            
            //Assert
            act.Should().Throw<InvalidOperationException>();
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

        [Fact]
        public void UpdateOverdraftLimit()
        {
            throw new NotImplementedException();
        }
        
        [Fact]
        public void Close()
        {
            throw new NotImplementedException();
        }
        
        [Fact]
        public void ReOpen()
        {
            throw new NotImplementedException();
        }
        
    }
}