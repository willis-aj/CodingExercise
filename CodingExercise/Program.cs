using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, SourceGenerationContext.Default);
});


var app = builder.Build();
var myApi = app.MapGroup("/deposit");
myApi.MapPost("/", (HttpRequest request) =>
{
    var jsonDepositRequest = request.ReadFromJsonAsync<Bank.DepositRequest>();
    Bank.DepositRequest depositRequest = new Bank.DepositRequest(jsonDepositRequest.Result.CustomerId, jsonDepositRequest.Result.AccountId, jsonDepositRequest.Result.Amount);
    Bank bank = new Bank();
    Bank.DepositResponse response = bank.MakeDeposit(depositRequest);
    return JsonSerializer.Serialize(response, typeof(Bank.DepositResponse), SourceGenerationContext.Default);
});

app.Run();
public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Bank.DepositResponse))]
[JsonSerializable(typeof(Bank.DepositRequest))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}


class Bank
{
    Account[] sampleAccounts =
        {
            new(1, 1, 100),
            new(2, 2, 200),
            new(5, 17, 2175.13m)
        };
    public DepositResponse MakeDeposit(DepositRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (request.AccountId < 0) throw new ArgumentOutOfRangeException(nameof(request));
        if (request.CustomerId < 0) throw new ArgumentOutOfRangeException(nameof(request));
        if (request.Amount < 0) throw new ArgumentException(nameof(request));

        Account accountToAccess = sampleAccounts.SingleOrDefault(account => account.AccountId == request.AccountId && account.CustomerId == request.CustomerId);

        DepositResponse response = new DepositResponse(0,0,0,false);

        if (accountToAccess != null)
        {
            accountToAccess.Balance += request.Amount;
            response.CustomerId = accountToAccess.CustomerId;
            response.AccountId = accountToAccess.AccountId;
            response.Balance = accountToAccess.Balance;
            response.Succeeded = true;
        }
        return response;
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
    }
    public class DepositRequest
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DepositRequest(int customerId, int accountId, decimal amount)
        {
            CustomerId = customerId;
            AccountId = accountId;
            Amount = amount;
        }
    }
    public class DepositResponse
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public bool Succeeded { get; set; }
        public DepositResponse(int customerId, int accountId, decimal balance, bool succeeded)
        {
            CustomerId = customerId;
            AccountId = accountId;
            Balance = balance;
            Succeeded = succeeded;
        }
    }
}
