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
                var button = ChipButton("🥔", Color.FromArgb("#F4C86A"));
                button.Clicked += (_, _) =>
                {
                    if (selections[player][chipIndex])
                        selections[player][chipIndex] = false;
                    else if (selections[player].Count(chosen => chosen) < BombsPerPlayer)
                        selections[player][chipIndex] = true;

                    button.Text = selections[player][chipIndex] ? "💣" : "🥔";
                    button.BackgroundColor = selections[player][chipIndex] ? Color.FromArgb("#E56D5E") : Color.FromArgb("#F4C86A");
                    UpdateCounter();
                };
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
                var button = ChipButton(eaten[targetPlayer][index] ? (bombPositions[targetPlayer][index] ? "💥" : "✓") : "🥔",
                    eaten[targetPlayer][index] ? Color.FromArgb("#DDE4E8") : Color.FromArgb("#F4C86A"));
                button.IsEnabled = !eaten[targetPlayer][index];
                button.Clicked += async (_, _) =>
                {
                    if (gameOver || eaten[targetPlayer][chipIndex]) return;
                    eaten[targetPlayer][chipIndex] = true;
                    if (bombPositions[targetPlayer][chipIndex])
                        bombsEaten[currentPlayer]++;

                    await button.ScaleToAsync(0.82, 70);
                    if (bombPositions[targetPlayer][chipIndex])
                    {
                        button.Text = "💥";
                        button.BackgroundColor = Color.FromArgb("#F6B2A8");
                        status.Text = $"{T("Bombe !")} {bombsEaten[currentPlayer]} / 3";
                    }
                    else
                    {
                        button.Text = "✓";
                        button.BackgroundColor = Color.FromArgb("#DDE4E8");
                        status.Text = T("Chip sans bombe.");
                    }
                    button.IsEnabled = false;

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
                };
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

    private static Button ChipButton(string text, Color color) => new()
    {
        Text = text,
        FontSize = 27,
        Padding = 0,
        BackgroundColor = color,
        TextColor = Colors.White,
        CornerRadius = 22,
        BorderColor = Color.FromArgb("#FFFDF5"),
        BorderWidth = 2,
        Shadow = new Shadow { Brush = Color.FromArgb("#3A664A2A"), Offset = new Point(0, 3), Radius = 7, Opacity = 0.28f }
    };
}
