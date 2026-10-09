namespace Kaskrout;

public sealed class DiceGame(IGameUiHost host) : GameModuleBase(host)
{
    public override string Id => GameIds.Dice;

    public override void Launch()
    {
        StartPage("Lance le dé", "Lance le dé et essaie d’obtenir un six !");
        var die = new WebView
        {
            Source = new HtmlWebViewSource { Html = Dice3D.Page },
            HeightRequest = 280,
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center
        };
        var dieReady = false;
        die.Navigated += (_, _) => dieReady = true;
        var result = Text("À toi de jouer !", 20, true, Muted);
        var rollButton = MakeButton("Lancer le dé", Green, () => { }, 58);
        var rolling = false;
        rollButton.Clicked += async (_, _) =>
        {
            if (rolling || !dieReady) return;
            rolling = true;
            rollButton.IsEnabled = false;
            result.Text = T("Ça tourne…");
            var number = random.Next(1, 7);
            await die.EvaluateJavaScriptAsync($"rollDice({number})");
            await Task.Delay(1750);
            result.Text = F($"Résultat : {number}", $"Result: {number}", $"Resultaat: {number}");
            if (number == 6)
                AddPoints(3);
            rollButton.IsEnabled = true;
            rolling = false;
        };
        body.Children.Add(Panel(new VerticalStackLayout
        {
            Spacing = 12,
            Padding = new Thickness(10, 18),
            Children = { die, result }
        }, Color.FromArgb("#FFF0EE")));
        body.Children.Add(rollButton);
    }
}
