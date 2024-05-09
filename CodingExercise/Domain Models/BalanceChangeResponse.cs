public class BalanceChangeResponse
{
    public int CustomerId { get; set; }
    public int AccountId { get; set; }
    public decimal Balance { get; set; }
    public bool Succeeded { get; set; }
    public BalanceChangeResponse(int customerId, int accountId, decimal balance, bool succeeded)
    {
        CustomerId = customerId;
        AccountId = accountId;
        Balance = balance;
        Succeeded = succeeded;
    }
}