public class BalanceChangeRequest
{
    public int CustomerId { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public BalanceChangeRequest(int customerId, int accountId, decimal amount)
    {
        CustomerId = customerId;
        AccountId = accountId;
        Amount = amount;
    }
}