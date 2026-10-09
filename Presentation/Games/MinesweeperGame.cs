namespace Kaskrout;

public sealed class MinesweeperGame(IGameUiHost host) : GameModuleBase(host)
{
    public override string Id => GameIds.Minesweeper;

    public override void Launch()
    {
        const int size = 9;
        const int mineTotal = 10;
        StartPage("Démineur", "Repère les cases sûres et évite les mines.");

        var mines = new bool[size, size];
        var opened = new bool[size, size];
        var flagged = new bool[size, size];
        var cells = new Button[size, size];
        var generated = false;
        var gameOver = false;
        var flagMode = false;
        var openedCount = 0;
        var flagCount = 0;
        var elapsed = 0;
        var hitRow = -1;
        var hitColumn = -1;
        var status = Text(T("Première case sûre. À toi de jouer !"), 14, true, Muted);

        Label Counter(string label, string color) => new()
        {
            Text = label, FontSize = 16, FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb(color), BackgroundColor = Color.FromArgb("#25334A"),
            HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center,
            WidthRequest = 90, HeightRequest = 42, Margin = new Thickness(3), Padding = new Thickness(7, 4)
        };
        var mineCounter = Counter("💣 010", "#FFB86B");
        var clock = Counter("⏱ 000", "#8EE6CE");
        var reset = new Button
        {
            Text = "↻", FontSize = 27, WidthRequest = 48, HeightRequest = 48,
            Padding = 0, Margin = new Thickness(3), CornerRadius = 17,
            BackgroundColor = Color.FromArgb("#715CE8"), TextColor = Colors.White,
            Shadow = new Shadow { Brush = Color.FromArgb("#55715CE8"), Offset = new Point(0, 3), Radius = 7, Opacity = 0.35f }
        };
        reset.Clicked += (_, _) => Launch();
        var header = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            },
            BackgroundColor = Colors.Transparent, Padding = 5, ColumnSpacing = 5
        };
        header.Add(mineCounter, 0, 0);
        header.Add(reset, 1, 0);
        header.Add(clock, 2, 0);

        var grid = new Grid { RowSpacing = 4, ColumnSpacing = 4, HeightRequest = 340 };
        for (var i = 0; i < size; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        }

        Button flagButton = null!;
        flagButton = MakeButton(T("Drapeaux : désactivés"), Purple, () =>
        {
            flagMode = !flagMode;
            flagButton.Text = T(flagMode ? "Drapeaux : activés" : "Drapeaux : désactivés");
            flagButton.BackgroundColor = flagMode ? Green : Purple;
            status.Text = T(flagMode ? "Drapeaux : activés" : "Première case sûre. À toi de jouer !");
        }, 50);

        int NeighbourMines(int row, int column)
        {
            var count = 0;
            for (var r = Math.Max(0, row - 1); r <= Math.Min(size - 1, row + 1); r++)
                for (var c = Math.Max(0, column - 1); c <= Math.Min(size - 1, column + 1); c++)
                    if ((r != row || c != column) && mines[r, c]) count++;
            return count;
        }

        void GenerateMines(int safeRow, int safeColumn)
        {
            var choices = new List<(int Row, int Column)>();
            for (var row = 0; row < size; row++)
                for (var column = 0; column < size; column++)
                    if (Math.Abs(row - safeRow) > 1 || Math.Abs(column - safeColumn) > 1)
                        choices.Add((row, column));
            foreach (var choice in choices.OrderBy(_ => random.Next()).Take(mineTotal))
                mines[choice.Row, choice.Column] = true;
            generated = true;
        }

        void Reveal(int row, int column)
        {
            if (row < 0 || row >= size || column < 0 || column >= size || opened[row, column] || flagged[row, column] || mines[row, column]) return;
            opened[row, column] = true;
            openedCount++;
            if (NeighbourMines(row, column) != 0) return;
            for (var r = row - 1; r <= row + 1; r++)
                for (var c = column - 1; c <= column + 1; c++)
                    if (r != row || c != column) Reveal(r, c);
        }

        void RefreshBoard()
        {
            mineCounter.Text = $"💣 {(mineTotal - flagCount):000}";
            clock.Text = $"⏱ {Math.Min(elapsed, 999):000}";
            for (var row = 0; row < size; row++)
                for (var column = 0; column < size; column++)
                {
                    var cell = cells[row, column];
                    cell.IsEnabled = !gameOver;
                    cell.BackgroundColor = opened[row, column]
                        ? Color.FromArgb("#F3F5FA")
                        : flagged[row, column] ? Color.FromArgb("#FFE5BC") : Color.FromArgb("#607CE5");
                    cell.Text = flagged[row, column] ? "🚩"
                        : opened[row, column] && mines[row, column] ? "💣"
                        : opened[row, column] && NeighbourMines(row, column) > 0 ? NeighbourMines(row, column).ToString()
                        : "";
                    cell.TextColor = opened[row, column] && !mines[row, column]
                        ? NeighbourColor(NeighbourMines(row, column))
                        : Ink;
                    if (row == hitRow && column == hitColumn)
                        cell.BackgroundColor = Color.FromArgb("#F28B82");
                }
        }

        void Finish(bool won)
        {
            gameOver = true;
            reset.Text = "↻";
            if (won)
            {
                for (var row = 0; row < size; row++)
                    for (var column = 0; column < size; column++)
                        if (!mines[row, column] && !opened[row, column])
                        {
                            opened[row, column] = true;
                            openedCount++;
                        }
                status.Text = T("Grille nettoyée ! +10 points 🎉");
                AddPoints(10);
                ShowGameFeedback(true, F("Grille nettoyée ! +10 points", "Board cleared! +10 points", "Bord leeggemaakt! +10 punten"));
            }
            else
            {
                status.Text = T("Mines révélées ! Recommence pour tenter ta chance.");
                ShowGameFeedback(false, F("Une mine a explosé. Recommence !", "A mine exploded. Try again!", "Een mijn is ontploft. Probeer opnieuw!"));
            }
            RefreshBoard();
        }

        for (var row = 0; row < size; row++)
            for (var column = 0; column < size; column++)
            {
                var cellRow = row;
                var cellColumn = column;
                var cell = new Button
                {
                    Text = "", FontSize = 16, FontAttributes = FontAttributes.Bold,
                    Padding = 0, Margin = 0, CornerRadius = 8,
                    BackgroundColor = Color.FromArgb("#607CE5"), TextColor = Colors.White,
                    Shadow = new Shadow { Brush = Color.FromArgb("#1C25334A"), Offset = new Point(0, 2), Radius = 3, Opacity = 0.22f }
                };
                cell.Clicked += async (_, _) =>
                {
                    if (gameOver) return;
                    if (flagMode)
                    {
                        if (!opened[cellRow, cellColumn])
                        {
                            flagged[cellRow, cellColumn] = !flagged[cellRow, cellColumn];
                            flagCount += flagged[cellRow, cellColumn] ? 1 : -1;
                            RefreshBoard();
                        }
                        return;
                    }

                    if (flagged[cellRow, cellColumn] || opened[cellRow, cellColumn]) return;
                    if (!generated) GenerateMines(cellRow, cellColumn);
                    if (mines[cellRow, cellColumn])
                    {
                        hitRow = cellRow;
                        hitColumn = cellColumn;
                        for (var r = 0; r < size; r++)
                            for (var c = 0; c < size; c++)
                                if (mines[r, c]) opened[r, c] = true;
                        Finish(false);
                        return;
                    }

                    await cell.ScaleToAsync(0.88, 60);
                    await cell.ScaleToAsync(1, 130, Easing.SpringOut);
                    Reveal(cellRow, cellColumn);
                    if (openedCount == size * size - mineTotal) Finish(true);
                    else
                    {
                        status.Text = T("Première case sûre. À toi de jouer !");
                        RefreshBoard();
                    }
                };
                cells[row, column] = cell;
                grid.Add(cell, column, row);
            }

        var boardPanel = Panel(new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                header,
                new Border
                {
                    BackgroundColor = Color.FromArgb("#DDE4F4"), StrokeThickness = 0,
                    StrokeShape = new RoundRectangle { CornerRadius = 18 }, Padding = 7, Content = grid
                }
            }
        }, Color.FromArgb("#EFF2FA"));
        body.Children.Add(boardPanel);
        body.Children.Add(flagButton);
        body.Children.Add(status);
        RefreshBoard();

        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (gameOver || !body.Children.Contains(boardPanel)) return false;
            if (!generated) return true;
            elapsed++;
            clock.Text = $"⏱ {Math.Min(elapsed, 999):000}";
            return !gameOver;
        });
    }

    static Color NeighbourColor(int count) => count switch
    {
        1 => Color.FromArgb("#0000FF"),
        2 => Color.FromArgb("#008000"),
        3 => Color.FromArgb("#FF0000"),
        4 => Color.FromArgb("#000080"),
        5 => Color.FromArgb("#800000"),
        6 => Color.FromArgb("#008080"),
        _ => Color.FromArgb("#202020")
    };
}
