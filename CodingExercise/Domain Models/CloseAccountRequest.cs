public class CloseRequest
{
    public int CustomerId { get; set; }
    public int AccountId { get; set; }
    public CloseRequest(int customerId, int accountId)
    {
        CustomerId = customerId;
        AccountId = accountId;
    }
}