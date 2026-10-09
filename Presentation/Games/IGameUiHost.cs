namespace Kaskrout;

public interface IGameUiHost
{
    VerticalStackLayout Body { get; }
    Random Random { get; }
    IDispatcher Dispatcher { get; }
    IGameSettingsStore Settings { get; }
    IPointsService Points { get; }
    string Translate(string value);
    string Format(string french, string english, string dutch);
    Label CreateText(string value, double size, bool bold = false, Color? color = null);
    Button CreateButton(string label, Color color, Action action, double height = 58);
    Border CreatePanel(View content, Color? color = null);
    void StartPage(string title, string subtitle);
    void ShowGameFeedback(bool won, string message, bool temporary = false);
}
