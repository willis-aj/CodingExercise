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

myApi.MapPost("/closeAccount", (HttpRequest request) =>
{
    var jsonCloseRequest = request.ReadFromJsonAsync<CloseRequest>();
    CloseRequest closeRequest = new CloseRequest(jsonCloseRequest.Result.CustomerId, jsonCloseRequest.Result.AccountId);

    return Results.Json(Bank.CloseAccount(closeRequest), typeof(CloseResponse), SourceGenerationContext.Default);
});
app.Run();

[JsonSerializable(typeof(BalanceChangeRequest))]
[JsonSerializable(typeof(BalanceChangeResponse))]
[JsonSerializable(typeof(CloseRequest))]
[JsonSerializable(typeof(CloseResponse))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}


public class Bank
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
    public static CloseResponse CloseAccount(CloseRequest request)
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

public class Account
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
