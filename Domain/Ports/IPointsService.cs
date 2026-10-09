namespace Kaskrout;

public interface IPointsService
{
    int Balance { get; }
    void Add(int amount);
}

public sealed class PreferencesPointsService(IGameSettingsStore settings) : IPointsService
{
    private const string BalanceKey = "points";
    public int Balance => settings.GetInt(BalanceKey);

    public void Add(int amount)
    {
        if (amount <= 0) return;
        settings.SetInt(BalanceKey, Balance + amount);
    }
}
