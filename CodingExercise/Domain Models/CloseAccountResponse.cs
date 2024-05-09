public class CloseResponse
{
    public int CustomerId { get; set; }
    public int AccountId { get; set; }
    public bool Succeeded { get; set; }
    public CloseResponse(int customerId, int accountId, bool succeeded)
    {
        CustomerId = customerId;
        AccountId = accountId;
        Succeeded = succeeded;
    }
}
