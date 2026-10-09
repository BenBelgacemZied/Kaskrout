namespace Kaskrout;

public interface IGameSettingsStore
{
    int GetInt(string key, int fallback = 0);
    void SetInt(string key, int value);
}

public sealed class PreferencesGameSettingsStore : IGameSettingsStore
{
    public int GetInt(string key, int fallback = 0) => Preferences.Default.Get(key, fallback);
    public void SetInt(string key, int value) => Preferences.Default.Set(key, value);
}
