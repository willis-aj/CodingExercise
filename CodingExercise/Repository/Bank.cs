public class Bank:IBank
{
    //would have liked to make a factory to generate different kinds of accounts (checking, savings, etc.)
    //this is pretend database
    static Account[] sampleAccounts =
        {
            new(1, 1, 100, true),
            new(2, 2, 200, true),
            new(5, 17, 2175.13m, true),
            new(7, 7, 0, true)
        };

    public BalanceChangeResponse MakeDeposit(BalanceChangeRequest request)
    {
        try
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.AccountId < 0) throw new ArgumentOutOfRangeException(nameof(request));
            if (request.CustomerId < 0) throw new ArgumentOutOfRangeException(nameof(request));
            if (request.Amount < 0) throw new ArgumentException(nameof(request));

            Account accountToAccess = sampleAccounts.SingleOrDefault(account => account.AccountId == request.AccountId && account.CustomerId == request.CustomerId); //this would be SQL in the real world

            if (accountToAccess == null) throw new Exception();
            accountToAccess.Deposit(request.Amount);
            BalanceChangeResponse response = new BalanceChangeResponse(accountToAccess.CustomerId, accountToAccess.AccountId, accountToAccess.Balance, true);
            return response;
        }
        catch (Exception)
        {
            return new BalanceChangeResponse(0, 0, 0, false);
        }
    }
    public BalanceChangeResponse MakeWithdrawl(BalanceChangeRequest request)
    {
        try
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.AccountId < 0) throw new ArgumentOutOfRangeException(nameof(request));
            if (request.CustomerId < 0) throw new ArgumentOutOfRangeException(nameof(request));
            if (request.Amount < 0) throw new ArgumentException(nameof(request));

            Account accountToAccess = sampleAccounts.SingleOrDefault(account => account.AccountId == request.AccountId && account.CustomerId == request.CustomerId); //this would be SQL in the real world

            if (accountToAccess == null) throw new Exception();
            accountToAccess.Withdrawl(request.Amount);
            BalanceChangeResponse response = new BalanceChangeResponse(accountToAccess.CustomerId, accountToAccess.AccountId, accountToAccess.Balance, true);
            return response;
        }
        catch (Exception)
        {
            return new BalanceChangeResponse(0, 0, 0, false);
        }
    }
    public CloseResponse CloseAccount(CloseRequest request)
    {
        try
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.AccountId < 0) throw new ArgumentOutOfRangeException(nameof(request));
            if (request.CustomerId < 0) throw new ArgumentOutOfRangeException(nameof(request));

            Account accountToAccess = sampleAccounts.SingleOrDefault(account => account.AccountId == request.AccountId && account.CustomerId == request.CustomerId); //this would be SQL in the real world

            if (accountToAccess == null) throw new Exception();
            if (accountToAccess.Balance != 0) throw new Exception();
            accountToAccess.Close(); // this would be SQL in the real world
            CloseResponse response = new CloseResponse(accountToAccess.CustomerId, accountToAccess.AccountId, true);
            return response;
        }
        catch (Exception)
        {
            return new CloseResponse(0, 0, false);
        }
    }
}