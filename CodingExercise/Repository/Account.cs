public class Account : IAccount
{
    public int CustomerId { get; set; }
    public int AccountId { get; set; }
    public decimal Balance { get; set; }
    public bool Active { get; set; }

    public Account(int customerId, int accountId, decimal balance, bool active)
    {
        CustomerId = customerId;
        AccountId = accountId;
        Balance = balance;
        Active = active;
    }
    public void Deposit(decimal amount)
    {
        Balance += amount;
    }
    public void Withdrawl(decimal amount)
    {
        Balance -= amount;
    }
    public void Close()
    {
        Active = false;
    }
}
