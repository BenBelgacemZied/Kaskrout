namespace Kaskrout;

public sealed class PairsGame(IGameUiHost host) : GameModuleBase(host)
{
    public override string Id => GameIds.Pairs;

    public override void Launch()
    {
        StartPage("Jeu des paires", "Retourne deux cartes et retrouve les images identiques.");
        var symbols = new[] { "🐱", "🍓", "🚀", "🐸", "🌈", "🍕", "🐱", "🍓", "🚀", "🐸", "🌈", "🍕" }
            .OrderBy(_ => random.Next()).ToArray();
        var open = new List<int>(2);
        var matched = new HashSet<int>();
        var busy = false;
        var matches = 0;
        var status = Text("Paires trouvées : 0 / 6", 17, true, Purple);
        var grid = new Grid { RowSpacing = 9, ColumnSpacing = 9, HeightRequest = 360 };
        for (var i = 0; i < 4; i++) grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
        for (var i = 0; i < 3; i++) grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        var cards = new Button[12];
        for (var i = 0; i < 12; i++)
        {
            var index = i;
            var card = new Button { Text = "✦", FontSize = 29, FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromArgb("#715CE8"), TextColor = Colors.White, CornerRadius = 20,
                Shadow = new Shadow { Brush = Color.FromArgb("#65715CE8"), Offset = new Point(0, 5), Radius = 9, Opacity = 0.34f } };
            card.Clicked += async (_, _) =>
            {
                if (busy || matched.Contains(index) || open.Contains(index)) return;
                await card.RotateYToAsync(90, 90);
                card.Text = symbols[index]; card.BackgroundColor = Colors.White;
                card.TextColor = Color.FromArgb("#D93D65");
                card.FontSize = 39; open.Add(index);
                await card.RotateYToAsync(0, 100);
                if (open.Count < 2) return;
                busy = true;
                await Task.Delay(650);
                var first = open[0]; var second = open[1]; open.Clear();
                if (symbols[first] == symbols[second])
                {
                    matched.Add(first); matched.Add(second); matches++; AddPoints(2);
                    cards[first].BackgroundColor = Color.FromArgb("#CFF5E7");
                    cards[second].BackgroundColor = Color.FromArgb("#CFF5E7");
                    await cards[first].ScaleToAsync(1.08, 100); await cards[first].ScaleToAsync(1, 160, Easing.SpringOut);
                    await cards[second].ScaleToAsync(1.08, 100); await cards[second].ScaleToAsync(1, 160, Easing.SpringOut);
                    status.Text = matches == 6 ? T("Toutes les paires trouvées ! 🎉") : F($"Paires trouvées : {matches} / 6", $"Pairs found: {matches} / 6", $"Paren gevonden: {matches} / 6");
                    if (matches == 6)
                    {
                        AddPoints(8);
                        ShowGameFeedback(true, F("Toutes les paires sont trouvées !", "You found every pair!", "Je hebt alle paren gevonden!"));
                    }
                }
                else
                {
                    cards[first].Text = "✦"; cards[second].Text = "✦";
                    foreach (var missed in new[] { cards[first], cards[second] })
                    {
                        await missed.RotateYToAsync(90, 80);
                        missed.BackgroundColor = Color.FromArgb("#715CE8"); missed.TextColor = Colors.White; missed.FontSize = 29;
                        await missed.RotateYToAsync(0, 100);
                    }
                }
                busy = false;
            };
            cards[i] = card; grid.Add(card, i % 3, i / 3);
        }
        body.Children.Add(status);
        body.Children.Add(Panel(grid));
        body.Children.Add(MakeButton("Nouvelle partie", Purple, Launch));
    }
}
