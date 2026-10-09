namespace Kaskrout;

public sealed class ReflexGame(IGameUiHost host) : GameModuleBase(host)
{
    public override string Id => GameIds.Reflex;

    bool running;
    bool armed;
    DateTime startAt;

    public override void Launch()
    {
        StartPage("Réflexe", "Attends le vert… puis appuie vite !");
        armed = false; running = true;
        var recordMs = GetIntSetting("reflex-record-ms", 0);
        string RecordText() => recordMs == 0
            ? F("Record à battre : aucun", "Record to beat: none", "Te verbreken record: geen")
            : F($"Record à battre : {recordMs} ms", $"Record to beat: {recordMs} ms", $"Te verbreken record: {recordMs} ms");
        var recordLabel = Text(RecordText(), 15, true, Purple);
        var status = Text("Patiente un instant…", 18, true, Muted);
        Button circle = null!;
        circle = MakeButton("🟠", Color.FromArgb("#ED7C55"), () =>
        {
            if (!running) return;
            if (!armed)
            {
                running = false; status.Text = T("Trop tôt ! Essaie encore."); circle.Text = "🙈";
                ShowGameFeedback(false, F("Tu as appuyé trop tôt.", "You tapped too early.", "Je tikte te vroeg."));
                body.Children.Add(MakeButton("Rejouer", Purple, Launch)); return;
            }
            running = false;
            var milliseconds = (DateTime.UtcNow - startAt).TotalMilliseconds;
            var reward = Math.Max(1, 10 - (int)(milliseconds / 100)); AddPoints(reward);
            var newRecord = recordMs == 0 || milliseconds < recordMs;
            if (newRecord)
            {
                recordMs = (int)Math.Round(milliseconds);
                SetIntSetting("reflex-record-ms", recordMs);
                recordLabel.Text = RecordText();
            }
            status.Text = F($"{milliseconds:0} ms — +{reward} points !", $"{milliseconds:0} ms — +{reward} points!", $"{milliseconds:0} ms — +{reward} punten!");
            var feedback = newRecord
                ? F($"Nouveau record : {recordMs} ms !", $"New record: {recordMs} ms!", $"Nieuw record: {recordMs} ms!")
                : F($"Temps : {milliseconds:0} ms. Record à battre : {recordMs} ms.", $"Time: {milliseconds:0} ms. Record to beat: {recordMs} ms.", $"Tijd: {milliseconds:0} ms. Te verbreken record: {recordMs} ms.");
            ShowGameFeedback(true, feedback);
            body.Children.Add(MakeButton("Encore", Purple, Launch));
        }, 230);
        circle.FontSize = 70;
        circle.Shadow = new Shadow { Brush = Color.FromArgb("#50ED7C55"), Offset = new Point(0, 8), Radius = 16, Opacity = 0.38f };
        body.Children.Add(Panel(circle, Color.FromArgb("#FFF0EB")));
        body.Children.Add(recordLabel);
        body.Children.Add(status);
        Task.Delay(random.Next(1500, 4500)).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
        {
            if (!running) return;
            armed = true; startAt = DateTime.UtcNow; circle.Text = "🟢";
            circle.BackgroundColor = Green; status.Text = T("MAINTENANT !");
        }));
    }
}
