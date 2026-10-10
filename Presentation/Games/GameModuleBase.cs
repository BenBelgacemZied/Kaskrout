namespace Kaskrout;

public abstract class GameModuleBase(IGameUiHost host) : IGameModule
{
    protected IGameUiHost Host { get; } = host;
    protected VerticalStackLayout body => Host.Body;
    protected Random random => Host.Random;
    protected static readonly Color Ink = Color.FromArgb("#202743");
    protected static readonly Color Muted = Color.FromArgb("#7D849B");
    protected static readonly Color Paper = Color.FromArgb("#F7F6FC");
    protected static readonly Color Purple = Color.FromArgb("#715CE8");
    protected static readonly Color Green = Color.FromArgb("#31A98B");

    public abstract string Id { get; }
    public abstract void Launch();

    protected string T(string value) => Host.Translate(value);
    protected string F(string french, string english, string dutch) => Host.Format(french, english, dutch);
    protected Label Text(string value, double size, bool bold = false, Color? color = null) => Host.CreateText(value, size, bold, color);
    protected Button MakeButton(string label, Color color, Action action, double height = 58) => Host.CreateButton(label, color, action, height);
    protected Border Panel(View content, Color? color = null) => Host.CreatePanel(content, color);
    protected void StartPage(string title, string subtitle) => Host.StartPage(title, subtitle);
    protected void ShowGameFeedback(bool won, string message, bool temporary = false) => Host.ShowGameFeedback(won, message, temporary);
    protected IWalletService Wallet => Host.Wallet;
    protected void AddPoints(int amount)
    {
        Host.Points.Add(amount);
        Host.Wallet.Credit(amount);
    }
    protected int GetIntSetting(string key, int fallback = 0) => Host.Settings.GetInt(key, fallback);
    protected void SetIntSetting(string key, int value) => Host.Settings.SetInt(key, value);
}
