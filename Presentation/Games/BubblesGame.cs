namespace Kaskrout;

public sealed class BubblesGame(IGameUiHost host) : GameModuleBase(host)
{
    private const int Rows = 8;
    private const int Columns = 8;
    private static readonly Color[] BubbleColors =
    [
        Color.FromArgb("#F16D8B"),
        Color.FromArgb("#62C8A8"),
        Color.FromArgb("#F3C85B"),
        Color.FromArgb("#69A8E8"),
        Color.FromArgb("#A78AE8")
    ];

    public override string Id => GameIds.Bubbles;

    public override void Launch()
    {
        StartPage("Bulles zen", "Éclate les bulles de même couleur.");
        var level = Math.Max(1, GetIntSetting("bubbles-level", 1));
        var levelScore = 0;
        var target = 120 + (level - 1) * 40;
        var board = new int[Rows, Columns];
        var status = Text("Choisis un groupe d’au moins deux bulles.", 15, true, Purple);
        var scoreLabel = Text("", 16, true, Ink);
        var progress = new ProgressBar { ProgressColor = Purple, BackgroundColor = Color.FromArgb("#E9E6F4"), HeightRequest = 7 };
        var grid = new Grid { RowSpacing = 5, ColumnSpacing = 5, HeightRequest = 360 };
        for (var row = 0; row < Rows; row++) grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        for (var column = 0; column < Columns; column++) grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        void NewBoard()
        {
            for (var row = 0; row < Rows; row++)
                for (var column = 0; column < Columns; column++)
                    board[row, column] = random.Next(BubbleColors.Length);
        }

        void UpdateScore()
        {
            scoreLabel.Text = $"{T("NIVEAU")} {level}   ·   {T("Bulles éclatées")} : {levelScore} / {target}";
            progress.Progress = Math.Min(1, levelScore / (double)target);
        }

        List<(int Row, int Column)> FindGroup(int startRow, int startColumn)
        {
            var color = board[startRow, startColumn];
            var found = new HashSet<(int Row, int Column)> { (startRow, startColumn) };
            var pending = new Queue<(int Row, int Column)>();
            pending.Enqueue((startRow, startColumn));
            var directions = new (int Row, int Column)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
            while (pending.Count > 0)
            {
                var current = pending.Dequeue();
                foreach (var direction in directions)
                {
                    var nextRow = current.Row + direction.Row;
                    var nextColumn = current.Column + direction.Column;
                    if (nextRow < 0 || nextRow >= Rows || nextColumn < 0 || nextColumn >= Columns) continue;
                    if (board[nextRow, nextColumn] == color && found.Add((nextRow, nextColumn)))
                        pending.Enqueue((nextRow, nextColumn));
                }
            }
            return found.ToList();
        }

        void ApplyGravityAndRefill()
        {
            for (var column = 0; column < Columns; column++)
            {
                var remaining = new List<int>();
                for (var row = Rows - 1; row >= 0; row--)
                    if (board[row, column] >= 0) remaining.Add(board[row, column]);

                var rowIndex = Rows - 1;
                foreach (var color in remaining) board[rowIndex--, column] = color;
                while (rowIndex >= 0) board[rowIndex--, column] = random.Next(BubbleColors.Length);
            }
        }

        void RenderBoard()
        {
            grid.Children.Clear();
            for (var row = 0; row < Rows; row++)
            for (var column = 0; column < Columns; column++)
            {
                var r = row;
                var c = column;
                var bubble = new Button
                {
                    Text = "●", FontSize = 25, Padding = 0,
                    BackgroundColor = BubbleColors[board[row, column]], TextColor = Colors.White,
                    CornerRadius = 18, BorderColor = Color.FromArgb("#80FFFFFF"), BorderWidth = 1.5,
                    Shadow = new Shadow { Brush = Color.FromArgb("#28745A92"), Offset = new Point(0, 2), Radius = 4, Opacity = 0.25f }
                };
                bubble.Clicked += async (_, _) =>
                {
                    var group = FindGroup(r, c);
                    if (group.Count < 2)
                    {
                        status.Text = T("Choisis un groupe d’au moins deux bulles.");
                        await bubble.ScaleToAsync(0.88, 70);
                        await bubble.ScaleToAsync(1, 110, Easing.SpringOut);
                        return;
                    }

                    await bubble.ScaleToAsync(0.82, 70);
                    levelScore += group.Count * group.Count * 5;
                    foreach (var cell in group) board[cell.Row, cell.Column] = -1;
                    ApplyGravityAndRefill();
                    UpdateScore();
                    RenderBoard();

                    if (levelScore >= target)
                    {
                        AddPoints(5);
                        ShowGameFeedback(true, $"{T("Niveau terminé !")} {T("NIVEAU")} {level} 🎉");
                        level++;
                        SetIntSetting("bubbles-level", level);
                        levelScore = 0;
                        target = 120 + (level - 1) * 40;
                        NewBoard();
                        UpdateScore();
                        RenderBoard();
                        status.Text = T("Nouveau niveau !");
                    }
                    else status.Text = T("Touchez pour jouer");
                };
                grid.Add(bubble, column, row);
            }
        }

        NewBoard();
        UpdateScore();
        RenderBoard();
        body.Children.Add(Panel(new VerticalStackLayout { Spacing = 8, Children = { scoreLabel, progress } }));
        body.Children.Add(Panel(grid, Color.FromArgb("#F0FAFC")));
        body.Children.Add(status);
        body.Children.Add(MakeButton("Recommencer le niveau", Purple, () =>
        {
            levelScore = 0;
            target = 120 + (level - 1) * 40;
            NewBoard();
            UpdateScore();
            RenderBoard();
            status.Text = T("Choisis un groupe d’au moins deux bulles.");
        }, 50));
    }
}
