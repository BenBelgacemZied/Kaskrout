using Microsoft.Maui.Controls.Shapes;

namespace Kaskrout;

public sealed class ChipsBombGame(IGameUiHost host) : GameModuleBase(host)
{
    private const int ChipsPerPlayer = 9;
    private const int BombsPerPlayer = 3;

    public override string Id => GameIds.ChipsBomb;

    public override void Launch()
    {
        StartPage("Chips piégées", "Cachez chacun 3 bombes parmi vos 9 chips.");
        var playArea = new VerticalStackLayout { Spacing = 13 };
        var bombPositions = new[] { new bool[ChipsPerPlayer], new bool[ChipsPerPlayer] };
        var selections = new[] { new bool[ChipsPerPlayer], new bool[ChipsPerPlayer] };
        var eaten = new[] { new bool[ChipsPerPlayer], new bool[ChipsPerPlayer] };
        var bombsEaten = new int[2];
        var currentPlayer = 1;
        var gameOver = false;
        var status = Text("Choisis une chip du plateau adverse.", 15, true, Purple);

        void ShowSetup(int player)
        {
            playArea.Children.Clear();
            playArea.Children.Add(Text($"{T("Joueur")} {player + 1} : {T("Choisis 3 chips piégées.")}", 17, true));
            var counter = Text("", 15, true, Purple);
            var grid = CreateChipGrid();
            Button confirm = null!;

            void UpdateCounter()
            {
                var count = selections[player].Count(chosen => chosen);
                counter.Text = $"{T("Bombes placées :")} {count} / {BombsPerPlayer}";
                if (confirm is not null) confirm.IsEnabled = count == BombsPerPlayer;
            }

            for (var index = 0; index < ChipsPerPlayer; index++)
            {
                var chipIndex = index;
                var button = ChipView(selections[player][chipIndex] ? "💣" : "", () =>
                {
                    if (selections[player][chipIndex]) selections[player][chipIndex] = false;
                    else if (selections[player].Count(chosen => chosen) < BombsPerPlayer) selections[player][chipIndex] = true;
                    ShowSetup(player);
                });

                grid.Add(button, chipIndex % 3, chipIndex / 3);
            }

            UpdateCounter();
            confirm = MakeButton("Confirmer mes bombes", Purple, () =>
            {
                if (selections[player].Count(chosen => chosen) != BombsPerPlayer) return;
                Array.Copy(selections[player], bombPositions[player], ChipsPerPlayer);
                if (player == 0)
                    ShowPassScreen(1, () => ShowSetup(1));
                else
                    ShowPassScreen(0, ShowTurn);
            }, 52);
            confirm.IsEnabled = selections[player].Count(chosen => chosen) == BombsPerPlayer;
            playArea.Children.Add(counter);
            playArea.Children.Add(Panel(grid, Color.FromArgb("#FFF8E8")));
            playArea.Children.Add(confirm);
        }

        void ShowPassScreen(int nextPlayer, Action continueAction)
        {
            playArea.Children.Clear();
            playArea.Children.Add(Panel(new VerticalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    Text($"🔄  {T("Passez le téléphone à Joueur")} {nextPlayer + 1}", 20, true),
                    Text("Appuyez quand le joueur est prêt.", 15, false, Muted)
                }
            }));
            playArea.Children.Add(MakeButton("Je suis prêt", Purple, continueAction, 52));
        }

        void ShowTurn()
        {
            if (gameOver) return;
            currentPlayer = 1 - currentPlayer;
            var targetPlayer = 1 - currentPlayer;
            playArea.Children.Clear();
            playArea.Children.Add(Text($"{T("Joueur")} {currentPlayer + 1}, choisis une chip du joueur {targetPlayer + 1}.", 17, true));
            playArea.Children.Add(Text($"{T("Bombes mangées")} : {bombsEaten[currentPlayer]} / 3", 15, true, Color.FromArgb("#D94D68")));
            playArea.Children.Add(Text($"{T("Joueur")} {targetPlayer + 1} : {9 - eaten[targetPlayer].Count(value => value)} chips restantes", 13, false, Muted));

            var grid = CreateChipGrid();
            for (var index = 0; index < ChipsPerPlayer; index++)
            {
                var chipIndex = index;
                View button = null!;
                button = ChipView(eaten[targetPlayer][index] ? (bombPositions[targetPlayer][index] ? "💥" : "✓") : "", async () =>
                {
                    if (gameOver || eaten[targetPlayer][chipIndex]) return;
                    eaten[targetPlayer][chipIndex] = true;
                    if (bombPositions[targetPlayer][chipIndex])
                        bombsEaten[currentPlayer]++;

                    if (bombPositions[targetPlayer][chipIndex])
                        status.Text = $"{T("Bombe !")} {bombsEaten[currentPlayer]} / 3";
                    else
                        status.Text = T("Chip sans bombe.");
                    if (button is Grid cell)
                        cell.Children.Add(new Label { Text = bombPositions[targetPlayer][chipIndex] ? "💥" : "✓",
                            FontSize = 25, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center,
                            TextColor = Color.FromArgb("#D94D45") });

                    if (bombsEaten[currentPlayer] >= BombsPerPlayer)
                    {
                        gameOver = true;
                        var winner = 1 - currentPlayer;
                        AddPoints(10);
                        status.Text = $"{T("Joueur")} {currentPlayer + 1} {T("a mangé ses trois bombes et perd !")} {T("Joueur")} {winner + 1} {T("remporte la partie !")}";
                        ShowGameFeedback(true, $"{T("Joueur")} {winner + 1} {T("remporte la partie !")}");
                        playArea.Children.Add(MakeButton("Rejouer", Purple, Launch, 52));
                        return;
                    }

                    await Task.Delay(650);
                    ShowPassScreen(1 - currentPlayer, ShowTurn);
                });
                grid.Add(button, chipIndex % 3, chipIndex / 3);
            }

            playArea.Children.Add(Panel(grid, Color.FromArgb("#FFF8E8")));
            status.Text = T("Choisis une chip du plateau adverse.");
        }

        body.Children.Add(playArea);
        body.Children.Add(status);
        ShowSetup(0);
    }

    private static Grid CreateChipGrid()
    {
        var grid = new Grid { RowSpacing = 8, ColumnSpacing = 8, HeightRequest = 300 };
        for (var i = 0; i < 3; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }
        return grid;
    }

    private static View ChipView(string text, Action tapped)
    {
        var grid = new Grid { WidthRequest = 78, HeightRequest = 70 };
        grid.Children.Add(new GraphicsView { Drawable = new CrispDrawable(), WidthRequest = 70, HeightRequest = 58,
            HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center });
        if (!string.IsNullOrEmpty(text))
            grid.Children.Add(new Label { Text = text, FontSize = 27, HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center, TextColor = Color.FromArgb("#D94D45") });
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => tapped();
        grid.GestureRecognizers.Add(tap);
        return grid;
    }
}

public sealed class CrispDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.Translate(dirtyRect.Center.X, dirtyRect.Center.Y);
        canvas.Rotate(-12);
        var w = dirtyRect.Width * .78f;
        var h = dirtyRect.Height * .50f;
        var x = -w / 2; var y = -h / 2;
        canvas.SetShadow(new SizeF(0, 2), 2, Color.FromArgb("#55452A10"));
        canvas.FillColor = Color.FromArgb("#D99A35");
        canvas.FillEllipse(x, y, w, h);
        canvas.SetShadow(new SizeF(0, 0), 0, Colors.Transparent);
        canvas.FillColor = Color.FromArgb("#F8D77A");
        canvas.FillEllipse(x + 2, y + 1, w - 4, h - 5);
        canvas.StrokeColor = Color.FromArgb("#E6B84F");
        canvas.StrokeSize = 1.2f;
        canvas.DrawEllipse(x + 5, y + 4, w - 10, h - 11);
        canvas.StrokeColor = Color.FromArgb("#FFE8A0");
        canvas.StrokeSize = 1.4f;
        for (var i = -1; i <= 1; i++)
        {
            var yy = i * h * .17f;
            canvas.DrawLine(-w * .30f, yy, -w * .12f, yy + h * .035f);
            canvas.DrawLine(-w * .12f, yy + h * .035f, w * .08f, yy - h * .025f);
            canvas.DrawLine(w * .08f, yy - h * .025f, w * .29f, yy + h * .01f);
        }
        canvas.FillColor = Color.FromArgb("#B87525");
        foreach (var p in new[] { new PointF(-.27f,-.12f), new PointF(.22f,-.10f), new PointF(-.08f,.10f), new PointF(.29f,.12f), new PointF(.03f,-.18f) })
            canvas.FillCircle(p.X * w, p.Y * h, Math.Max(1.2f, w * .018f));
        canvas.RestoreState();
    }
}
