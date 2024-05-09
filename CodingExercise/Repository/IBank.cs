public interface IBank
{
    BalanceChangeResponse MakeDeposit(BalanceChangeRequest request);
    BalanceChangeResponse MakeWithdrawl(BalanceChangeRequest request);
    CloseResponse CloseAccount(CloseRequest request);
}
