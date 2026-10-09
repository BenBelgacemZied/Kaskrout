namespace Kaskrout;

public sealed class PuzzleGame(IGameUiHost host) : GameModuleBase(host)
{
    public override string Id => GameIds.Puzzle;

    public override void Launch()
    {
        StartPage("Puzzle coulissant", "Fais glisser les nombres pour les ranger de 1 à 8.");
        var tiles = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        var empty = 8;
        var moves = 0;
        var status = Text("Coups : 0", 16, true, Purple);
        var grid = new Grid { RowSpacing = 7, ColumnSpacing = 7, HeightRequest = 300 };
        for (var i = 0; i < 3; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        }
        var buttons = new Button[9];
        for (var i = 0; i < 9; i++)
        {
            var index = i;
            var button = new Button { FontSize = 29, FontAttributes = FontAttributes.Bold, CornerRadius = 21,
                Shadow = new Shadow { Brush = Color.FromArgb("#65715CE8"), Offset = new Point(0, 5), Radius = 9, Opacity = 0.36f } };
            button.Clicked += async (_, _) =>
            {
                if (index == empty || !IsNeighbor(index, empty)) return;
                (tiles[index], tiles[empty]) = (tiles[empty], tiles[index]);
                empty = index; moves++; status.Text = F($"Coups : {moves}", $"Moves: {moves}", $"Zetten: {moves}");
                await button.ScaleToAsync(0.92, 65);
                await button.ScaleToAsync(1, 150, Easing.SpringOut);
                RefreshTiles();
                if (tiles.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 }))
                {
                    status.Text = F($"Bravo ! Puzzle terminé en {moves} coups 🎉", $"Great! Puzzle completed in {moves} moves 🎉", $"Goed gedaan! Puzzel opgelost in {moves} zetten 🎉");
                    AddPoints(10);
                    ShowGameFeedback(true, F($"Puzzle terminé en {moves} coups ! +10 points", $"Puzzle solved in {moves} moves! +10 points", $"Puzzel opgelost in {moves} zetten! +10 punten"));
                    foreach (var tile in buttons) tile.IsEnabled = false;
                }
            };
            buttons[i] = button;
            grid.Add(button, i % 3, i / 3);
        }
        void RefreshTiles()
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                buttons[i].Text = tiles[i] == 0 ? "" : tiles[i].ToString();
                buttons[i].BackgroundColor = tiles[i] == 0 ? Color.FromArgb("#E4F5F1") : (i % 3) switch
                {
                    0 => Color.FromArgb("#EEEAFE"), 1 => Color.FromArgb("#E5F2FF"), _ => Color.FromArgb("#FFF0E4")
                };
                buttons[i].TextColor = i % 3 == 1 ? Color.FromArgb("#3475C5") : Purple;
            }
        }
        // Mélange par mouvements légaux : le puzzle reste toujours résoluble.
        for (var step = 0; step < 36; step++)
        {
            var neighbors = Enumerable.Range(0, 9).Where(i => IsNeighbor(i, empty)).ToArray();
            var next = neighbors[random.Next(neighbors.Length)];
            (tiles[next], tiles[empty]) = (tiles[empty], tiles[next]);
            empty = next;
        }
        RefreshTiles();
        body.Children.Add(Panel(grid, Color.FromArgb("#FFFFFF")));
        body.Children.Add(status);
        body.Children.Add(MakeButton("Mélanger", Purple, () => Launch()));
    }

    static bool IsNeighbor(int first, int second) =>
        Math.Abs(first % 3 - second % 3) + Math.Abs(first / 3 - second / 3) == 1;
}
