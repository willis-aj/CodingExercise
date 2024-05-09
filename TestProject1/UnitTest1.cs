using Shouldly;

namespace TestProject
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void DepositTest()
        {
            // Arrange
            Account account = new Account(3, 3, 300, true);

            //Act
            account.Deposit(60);

            //Assert
            account.Balance.ShouldBe(360);
        }

        [TestMethod]
        public void WithdrawlTest()
        {
            // Arrange
            Account account = new Account(3, 3, 300, true);

            //Act
            account.Withdrawl(60);

            //Assert
            account.Balance.ShouldBe(240);
        }

        [TestMethod]
        public void CloseTest()
        {
            // Arrange
            Account account = new Account(3, 3, 300, true);

            //Act
            account.Close();

            //Assert
            account.Active.ShouldBe(false);
        }
    }

    [TestClass]
    public class BankTests
    {
        [TestMethod]
        public void DepositTest()
        {
            // Arrange
            BalanceChangeRequest request = new(1,1,50);
            IBank bank = new Bank();

            //Act
            BalanceChangeResponse response = bank.MakeDeposit(request);

            //Assert
            response.Balance.ShouldBe(150);
            response.AccountId.ShouldBe(1);
            response.CustomerId.ShouldBe(1);
            response.Succeeded.ShouldBeTrue();
        }

        [TestMethod]
        public void WithdrawlTest()
        {
            // Arrange
            BalanceChangeRequest request = new(2, 2, 50);
            IBank bank = new Bank();

            //Act
            BalanceChangeResponse response = bank.MakeWithdrawl(request);

            //Assert
            response.Balance.ShouldBe(150);
            response.AccountId.ShouldBe(2);
            response.CustomerId.ShouldBe(2);
            response.Succeeded.ShouldBeTrue();
        }

        [TestMethod]
        public void CloseTest()
        {
            // Arrange
            CloseRequest request = new(7, 7);
            IBank bank = new Bank();

            //Act
            CloseResponse response = bank.CloseAccount(request);

            //Assert
            response.AccountId.ShouldBe(7);
            response.CustomerId.ShouldBe(7);
            response.Succeeded.ShouldBeTrue();
        }
    }
}