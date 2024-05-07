using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, SourceGenerationContext.Default);
});


var app = builder.Build();
var myApi = app.MapGroup("/bank");
myApi.MapPost("/deposit", (HttpRequest request) =>
{
    var jsonDepositRequest = request.ReadFromJsonAsync<BalanceChangeRequest>();
    BalanceChangeRequest depositRequest = new BalanceChangeRequest(jsonDepositRequest.Result.CustomerId, jsonDepositRequest.Result.AccountId, jsonDepositRequest.Result.Amount);

    return Results.Json(Bank.MakeDeposit(depositRequest), typeof(BalanceChangeResponse), SourceGenerationContext.Default);
});

myApi.MapPost("/withdrawl", (HttpRequest request) =>
{
    var jsonWithdrawlRequest = request.ReadFromJsonAsync<BalanceChangeRequest>();
    BalanceChangeRequest withdrawlRequest = new BalanceChangeRequest(jsonWithdrawlRequest.Result.CustomerId, jsonWithdrawlRequest.Result.AccountId, jsonWithdrawlRequest.Result.Amount);

    return Results.Json(Bank.MakeWithdrawl(withdrawlRequest), typeof(BalanceChangeResponse), SourceGenerationContext.Default);
});
app.Run();

[JsonSerializable(typeof(BalanceChangeResponse))]
[JsonSerializable(typeof(BalanceChangeRequest))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}


class Bank
{
    //would have liked to make a factory to generate different kinds of accounts (checking, savings, etc.)
    //this is pretend db
    static Account[] sampleAccounts =
        {
            new(1, 1, 100),
            new(2, 2, 200),
            new(5, 17, 2175.13m)
        };

    public static BalanceChangeResponse MakeDeposit(BalanceChangeRequest request)
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
    public static BalanceChangeResponse MakeWithdrawl(BalanceChangeRequest request)
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
}

public class Account
{
    public int CustomerId { get; set; }
    public int AccountId { get; set; }
    public decimal Balance { get; set; }

    public Account(int customerId, int accountId, decimal balance)
    {
        CustomerId = customerId;
        AccountId = accountId;
        Balance = balance;
    }
    public void Deposit(decimal amount)
    {
        Balance += amount;
    }
    public void Withdrawl(decimal amount)
    {
        Balance -= amount;
    }
}
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
