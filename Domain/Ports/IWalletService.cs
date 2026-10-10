namespace Kaskrout;

public interface IWalletService
{
    int Balance { get; }
    void Credit(int amount);
    bool TrySpend(int amount);
}
