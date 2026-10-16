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
        var resultAwaitingNextTurn = false;
        var status = Text("Choisis une chip du plateau adverse.", 15, true, Purple);

        void ShowSetup(int player)
        {
            playArea.BackgroundColor = player == 0 ? Color.FromArgb("#EAF3FF") : Color.FromArgb("#FFF0DF");
            playArea.Padding = new Thickness(10);
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
                var button = ChipView(selections[player][chipIndex] ? "bomb" : "", () =>
                {
                    if (selections[player][chipIndex]) selections[player][chipIndex] = false;
                    else if (selections[player].Count(chosen => chosen) < BombsPerPlayer) selections[player][chipIndex] = true;
                    ShowSetup(player);
                });

                grid.Add(button, chipIndex % 3, chipIndex / 3);
            }

            UpdateCounter();
            confirm = MakeButton("Confirmer mes bombes", Color.FromArgb("#2864C5"), () =>
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
            playArea.BackgroundColor = Colors.Transparent;
            playArea.Padding = 0;
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
            resultAwaitingNextTurn = false;
            currentPlayer = 1 - currentPlayer;
            var targetPlayer = 1 - currentPlayer;
            var turnColor = currentPlayer == 0 ? Color.FromArgb("#EAF3FF") : Color.FromArgb("#FFF0DF");
            playArea.BackgroundColor = turnColor;
            playArea.Padding = new Thickness(10);
            playArea.Children.Clear();
            playArea.Children.Add(Panel(Text($"{T("Joueur")} {currentPlayer + 1}, choisis une chip du joueur {targetPlayer + 1}.", 17, true), turnColor));
            var bombsLabel = Text($"{T("Bombes mangées")} : {bombsEaten[currentPlayer]} / 3", 15, true, Color.FromArgb("#D94D68"));
            playArea.Children.Add(bombsLabel);
            playArea.Children.Add(Text($"{T("Joueur")} {targetPlayer + 1} : {9 - eaten[targetPlayer].Count(value => value)} chips restantes", 13, false, Muted));

            var grid = CreateChipGrid();
            for (var index = 0; index < ChipsPerPlayer; index++)
            {
                var chipIndex = index;
                View button = null!;
                button = ChipView(eaten[targetPlayer][index] ? (bombPositions[targetPlayer][index] ? "bomb" : "safe") : "", () =>
                {
                    if (gameOver || resultAwaitingNextTurn || eaten[targetPlayer][chipIndex]) return;
                    eaten[targetPlayer][chipIndex] = true;
                    if (bombPositions[targetPlayer][chipIndex])
                        bombsEaten[currentPlayer]++;
                    bombsLabel.Text = $"{T("Bombes mangées")} : {bombsEaten[currentPlayer]} / 3";

                    if (bombPositions[targetPlayer][chipIndex])
                    {
                        status.Text = $"{T("Bombe !")} {bombsEaten[currentPlayer]} / 3";
                        status.TextColor = Color.FromArgb("#C6283D");
                    }
                    else
                    {
                        status.Text = T("Chip sans bombe.");
                        status.TextColor = Color.FromArgb("#168A55");
                    }
                    if (button is Grid cell)
                        cell.Children.Add(ChipMarker(bombPositions[targetPlayer][chipIndex] ? "bomb" : "safe"));

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

                    resultAwaitingNextTurn = true;
                    foreach (var chipCell in grid.Children.OfType<VisualElement>())
                        chipCell.IsEnabled = false;
                    playArea.Children.Add(MakeButton("Joueur suivant", Purple, ShowTurn, 52));
                });
                grid.Add(button, chipIndex % 3, chipIndex / 3);
            }

            playArea.Children.Add(Panel(grid, turnColor));
            playArea.Children.Add(Text($"🔴 {T("Bombe")} • 🟢 {T("Chip sûre")}", 12, true, Color.FromArgb("#40516B")));
            status.Text = T("Choisis une chip du plateau adverse.");
            status.TextColor = Purple;
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
        var crisp = new Image { Source = "chips.png", Aspect = Aspect.AspectFit, WidthRequest = 70, HeightRequest = 58,
            HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => tapped();
        crisp.GestureRecognizers.Add(tap);
        grid.Children.Add(crisp);
        if (!string.IsNullOrEmpty(text))
            grid.Children.Add(ChipMarker(text));
        return grid;
    }

    private static View ChipMarker(string state)
    {
        var isBomb = state == "bomb";
        var badge = new Border
        {
            WidthRequest = 36,
            HeightRequest = 36,
            Padding = 0,
            BackgroundColor = Color.FromArgb(isBomb ? "#C6283D" : "#168A55"),
            Stroke = Colors.White,
            StrokeThickness = 2,
            StrokeShape = new RoundRectangle { CornerRadius = 18 },
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Shadow = new Shadow { Brush = Color.FromArgb("#66000000"), Offset = new Point(0, 2), Radius = 4, Opacity = 0.55f },
            Content = new Label
            {
                Text = isBomb ? "!" : "✓",
                FontSize = isBomb ? 24 : 22,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            }
        };
        return badge;
    }
}
