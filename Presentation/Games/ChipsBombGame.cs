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
        var crisp = new GraphicsView { Drawable = new CrispDrawable(), WidthRequest = 70, HeightRequest = 58,
            HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => tapped();
        crisp.GestureRecognizers.Add(tap);
        grid.Children.Add(crisp);
        if (!string.IsNullOrEmpty(text))
            grid.Children.Add(new Label { Text = text, FontSize = 27, HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center, InputTransparent = true, TextColor = Color.FromArgb("#D94D45") });
        return grid;
    }
}

public sealed class CrispDrawable : IDrawable
{
    static PathF ChipShape(float width, float height, float inset = 0)
    {
        var path = new PathF();
        const int points = 64;
        for (var i = 0; i <= points; i++)
        {
            var angle = (float)(Math.PI * 2 * i / points);
            var ripple = 1f + .045f * MathF.Sin(angle * 9f) + .018f * MathF.Sin(angle * 17f);
            var x = MathF.Cos(angle) * (width * .5f - inset) * ripple;
            var y = MathF.Sin(angle) * (height * .5f - inset) * ripple;
            if (i == 0) path.MoveTo(x, y);
            else path.LineTo(x, y);
        }
        path.Close();
        return path;
    }

    static void DrawSingleChip(ICanvas canvas, float width, float height, Color edge, Color center, float offsetX, float offsetY, float angle)
    {
        canvas.SaveState();
        canvas.Translate(offsetX, offsetY);
        canvas.Rotate(angle);
        canvas.SetShadow(new SizeF(0, 2), 2, Color.FromArgb("#6644260B"));
        canvas.FillColor = edge;
        canvas.FillPath(ChipShape(width, height));
        canvas.SetShadow(new SizeF(0, 0), 0, Colors.Transparent);
        canvas.FillColor = center;
        canvas.FillPath(ChipShape(width - 6, height - 6, 1));
        canvas.StrokeColor = Color.FromArgb("#D99B3C");
        canvas.StrokeSize = 1.1f;
        canvas.DrawPath(ChipShape(width - 5, height - 5, 1));
        canvas.StrokeColor = Color.FromArgb("#FFF0B2");
        canvas.StrokeSize = 1.5f;
        for (var row = -1; row <= 1; row++)
        {
            var y = row * height * .16f;
            canvas.DrawLine(-width * .30f, y, -width * .11f, y + height * .04f);
            canvas.DrawLine(-width * .11f, y + height * .04f, width * .08f, y - height * .035f);
            canvas.DrawLine(width * .08f, y - height * .035f, width * .29f, y + height * .012f);
        }
        canvas.FillColor = Color.FromArgb("#B87727");
        foreach (var p in new[] { new PointF(-.28f,-.12f), new PointF(.22f,-.13f), new PointF(-.07f,.12f), new PointF(.29f,.10f), new PointF(.02f,-.20f) })
            canvas.FillCircle(p.X * width, p.Y * height, Math.Max(1.2f, width * .017f));
        canvas.RestoreState();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.Translate(dirtyRect.Center.X, dirtyRect.Center.Y);
        var width = dirtyRect.Width * .82f;
        var height = dirtyRect.Height * .55f;
        DrawSingleChip(canvas, width * .90f, height * .94f, Color.FromArgb("#BA762E"), Color.FromArgb("#D99D42"),
            -width * .09f, height * .15f, -17);
        DrawSingleChip(canvas, width * .94f, height * .95f, Color.FromArgb("#C58131"), Color.FromArgb("#E8B954"),
            width * .08f, height * .07f, 11);
        DrawSingleChip(canvas, width, height, Color.FromArgb("#D79A35"), Color.FromArgb("#F8D77A"),
            -width * .02f, -height * .08f, -5);
        canvas.RestoreState();
    }
}
