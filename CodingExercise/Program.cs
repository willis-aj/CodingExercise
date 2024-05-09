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
    IBank bank = new Bank();

    return Results.Json(bank.MakeDeposit(depositRequest), typeof(BalanceChangeResponse), SourceGenerationContext.Default);
});

myApi.MapPost("/withdrawl", (HttpRequest request) =>
{
    var jsonWithdrawlRequest = request.ReadFromJsonAsync<BalanceChangeRequest>();
    BalanceChangeRequest withdrawlRequest = new BalanceChangeRequest(jsonWithdrawlRequest.Result.CustomerId, jsonWithdrawlRequest.Result.AccountId, jsonWithdrawlRequest.Result.Amount);
    IBank bank = new Bank();

    return Results.Json(bank.MakeWithdrawl(withdrawlRequest), typeof(BalanceChangeResponse), SourceGenerationContext.Default);
});

myApi.MapPost("/closeAccount", (HttpRequest request) =>
{
    var jsonCloseRequest = request.ReadFromJsonAsync<CloseRequest>();
    CloseRequest closeRequest = new CloseRequest(jsonCloseRequest.Result.CustomerId, jsonCloseRequest.Result.AccountId);
    IBank bank = new Bank();

    return Results.Json(bank.CloseAccount(closeRequest), typeof(CloseResponse), SourceGenerationContext.Default);
});
app.Run();

[JsonSerializable(typeof(BalanceChangeRequest))]
[JsonSerializable(typeof(BalanceChangeResponse))]
[JsonSerializable(typeof(CloseRequest))]
[JsonSerializable(typeof(CloseResponse))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}