namespace Kaskrout;

public sealed class PreferencesWalletService : IWalletService
{
    private const string BalanceKey = "wallet.balance";
    private const string InitializedKey = "wallet.initialized";
    public const int StartingBudget = 500;

    private readonly IGameSettingsStore settings;

    public PreferencesWalletService(IGameSettingsStore settings)
    {
        this.settings = settings;
        if (settings.GetInt(InitializedKey) == 0)
        {
            settings.SetInt(BalanceKey, StartingBudget);
            settings.SetInt(InitializedKey, 1);
        }
    }

    public int Balance => settings.GetInt(BalanceKey, StartingBudget);

    public void Credit(int amount)
    {
        if (amount <= 0) return;
        settings.SetInt(BalanceKey, Balance + amount);
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0 || Balance < amount) return false;
        settings.SetInt(BalanceKey, Balance - amount);
        return true;
    }
}
