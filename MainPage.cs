using Microsoft.Maui.Controls.Shapes;

namespace Kaskrout;

public class MainPage : ContentPage
{
    static readonly Color Ink = Color.FromArgb("#202743");
    static readonly Color Muted = Color.FromArgb("#7D849B");
    static readonly Color Paper = Color.FromArgb("#F7F6FC");
    static readonly Color Purple = Color.FromArgb("#715CE8");
    static readonly Color Green = Color.FromArgb("#31A98B");
    readonly VerticalStackLayout body = new() { Spacing = 16, Padding = new Thickness(20, 18, 20, 28) };
    readonly Random random = new();
    int points = Preferences.Default.Get("points", 0);
    int taps;
    int seconds;
    bool running;
    bool armed;
    DateTime startAt;

    public MainPage()
    {
        Title = "Kaskrout";
        BackgroundColor = Paper;
        Content = new ScrollView { Content = body };
        ShowHome();
    }

    Label Text(string value, double size, bool bold = false, Color? color = null) => new()
    {
        Text = value, FontSize = size,
        FontAttributes = bold ? FontAttributes.Bold : FontAttributes.None,
        TextColor = color ?? Ink, HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center
    };

    Button MakeButton(string label, Color color, Action action, double height = 58)
    {
        var button = new Button
        {
            Text = label, BackgroundColor = color, TextColor = Colors.White,
            CornerRadius = 18, HeightRequest = height, FontSize = 18,
            FontAttributes = FontAttributes.Bold
        };
        button.Clicked += (_, _) => action();
        return button;
    }

    void ShowHome()
    {
        running = false;
        body.Children.Clear();
        var hero = new Border
        {
            StrokeThickness = 0, BackgroundColor = Color.FromArgb("#EEEAFE"),
            StrokeShape = new RoundRectangle { CornerRadius = 28 },
            Padding = new Thickness(20, 22),
            Content = new VerticalStackLayout
            {
                Spacing = 7,
                Children =
                {
                    Text("✨  KASKROUT  ✨", 15, true, Purple),
                    Text("Une petite pause ?", 29, true),
                    Text("Choisis un mini-défi et amuse-toi !", 15, false, Muted)
                }
            }
        };
        body.Children.Add(hero);

        var scorePill = new Border
        {
            HorizontalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#F1E5B7"), StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 24 },
            Padding = new Thickness(18, 8),
            Content = Text($"⭐  {points} points", 17, true, Color.FromArgb("#BA841C"))
        };
        body.Children.Add(scorePill);
        body.Children.Add(Text("CHOISIS TON JEU", 13, true, Muted));

        var games = new (string Icon, string Title, string Detail, string Color, Action Play)[]
        {
            ("🧩", "Puzzle", "Remets les tuiles en ordre", "#EEEAFE", PlayPuzzle),
            ("👀", "Objet manquant", "Observe, puis retrouve-le", "#E4F5F1", PlayMissingObject),
            ("🃏", "Paires", "Associe les images identiques", "#FFF0E4", PlayPairs),
            ("🎯", "Attrape les étoiles", "Tape vite avant la fin", "#FFF7D9", PlayStars),
            ("🎲", "Lance le dé", "Un lancer porte-bonheur ?", "#E8F3FF", PlayDice),
            ("⚡", "Réflexe", "Attends le vert et appuie", "#FFE9EC", PlayReflex),
            ("🫧", "Éclate les bulles", "Fais-en éclater un maximum", "#E2F7FC", PlayBubbles),
            ("🪙", "Pile ou face", "La pièce choisit pour toi", "#FFF2D3", PlayCoin),
            ("🎰", "Machine surprise", "Quel emoji va sortir ?", "#FCE8F4", PlaySurprise)
        };
        var grid = new Grid { ColumnSpacing = 12, RowSpacing = 12 };
        grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        for (var i = 0; i < (games.Length + 1) / 2; i++)
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        for (var i = 0; i < games.Length; i++)
        {
            var game = games[i];
            var card = new Border
            {
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb(game.Color), StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 23 },
                Padding = new Thickness(14), HeightRequest = 158,
                Content = new VerticalStackLayout
                {
                    Spacing = 7, VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Border
                        {
                            WidthRequest = 48, HeightRequest = 48,
                            HorizontalOptions = LayoutOptions.Center,
                            BackgroundColor = Color.FromArgb(game.Color),
                            StrokeThickness = 0,
                            StrokeShape = new RoundRectangle { CornerRadius = 18 },
                            Content = Text(game.Icon, 27)
                        },
                        Text(game.Title, 15, true),
                        Text(game.Detail, 12, false, Muted)
                    }
                }
            };
            var tap = new TapGestureRecognizer { Command = new Command(game.Play) };
            card.GestureRecognizers.Add(tap);
            grid.Add(card, i % 2, i / 2);
        }
        body.Children.Add(grid);
    }

    void StartPage(string title, string subtitle)
    {
        running = false;
        body.Children.Clear();
        var top = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) } };
        var back = new Button
        {
            Text = "‹", FontSize = 32, TextColor = Purple, BackgroundColor = Colors.White,
            CornerRadius = 18, WidthRequest = 48, HeightRequest = 48, Padding = 0
        };
        back.Clicked += (_, _) => ShowHome();
        top.Add(back, 0, 0);
        top.Add(Text(title, 23, true), 1, 0);
        body.Children.Add(top);
        body.Children.Add(Text(subtitle, 15, false, Muted));
    }

    Border Panel(View content, Color? color = null) => new()
    {
        BackgroundColor = color ?? Colors.White,
        Stroke = Color.FromArgb("#ECEAF3"), StrokeThickness = 1,
        StrokeShape = new RoundRectangle { CornerRadius = 24 },
        Padding = new Thickness(16), Content = content
    };

    void PlayPuzzle()
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
            var button = new Button { FontSize = 28, FontAttributes = FontAttributes.Bold, CornerRadius = 17 };
            button.Clicked += (_, _) =>
            {
                if (index == empty || !IsNeighbor(index, empty)) return;
                (tiles[index], tiles[empty]) = (tiles[empty], tiles[index]);
                empty = index; moves++; status.Text = $"Coups : {moves}";
                RefreshTiles();
                if (tiles.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 }))
                {
                    status.Text = $"Bravo ! Puzzle terminé en {moves} coups 🎉";
                    AddPoints(10);
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
                buttons[i].BackgroundColor = tiles[i] == 0 ? Color.FromArgb("#F1EFF8") : Color.FromArgb("#EEEAFE");
                buttons[i].TextColor = Purple;
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
        body.Children.Add(MakeButton("Mélanger", Purple, () => PlayPuzzle()));
    }

    static bool IsNeighbor(int first, int second) =>
        Math.Abs(first % 3 - second % 3) + Math.Abs(first / 3 - second / 3) == 1;

    void PlayMissingObject()
    {
        StartPage("Objet manquant", "Observe les images, puis retrouve celle qui a disparu.");
        var round = 0;
        var score = 0;
        var rounds = new[]
        {
            new[] { "🍎", "🚲", "🐱", "⚽", "🌼" },
            new[] { "🍕", "🚀", "🐸", "🎈", "🧸" },
            new[] { "🍉", "🦋", "🚂", "🎸", "🌙" }
        };
        var roundArea = new VerticalStackLayout { Spacing = 12 };
        body.Children.Add(roundArea);
        void ShowRound()
        {
            if (!body.Children.Contains(roundArea)) return;
            roundArea.Children.Clear();
            if (round >= rounds.Length)
            {
                roundArea.Children.Add(Panel(new VerticalStackLayout
                {
                    Spacing = 12,
                    Children = { Text("Bien joué !", 26, true, Green), Text($"Tu as trouvé {score} objets sur {rounds.Length}.", 18) }
                }));
                roundArea.Children.Add(MakeButton("Rejouer", Purple, PlayMissingObject));
                AddPoints(score * 3);
                return;
            }

            var items = rounds[round];
            var missing = items[random.Next(items.Length)];
            var visible = Text(string.Join("   ", items), 31);
            var question = Text("Mémorise bien…", 16, true, Purple);
            var panel = Panel(new VerticalStackLayout { Spacing = 14, Children = { visible, question } });
            roundArea.Children.Add(panel);
            var optionsArea = new VerticalStackLayout { Spacing = 9 };
            roundArea.Children.Add(optionsArea);
            Task.Delay(1800).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!body.Children.Contains(roundArea) || !roundArea.Children.Contains(panel)) return;
                visible.Text = string.Join("   ", items.Where(x => x != missing));
                question.Text = "Lequel a disparu ?";
                var decoys = new[] { "🍋", "🛴", "🐶", "🎁", "☀️", "🍪", "🚕", "🐻", "🎾", "🍇" }
                    .Where(x => !items.Contains(x)).OrderBy(_ => random.Next()).Take(3).ToList();
                foreach (var option in decoys.Append(missing).OrderBy(_ => random.Next()))
                {
                    var answer = MakeButton(option, Color.FromArgb("#F2F0FB"), () =>
                    {
                        if (optionsArea.Children.All(x => !x.IsEnabled)) return;
                        if (option == missing) { score++; question.Text = "Exactement ! ✨"; }
                        else question.Text = $"C’était {missing} !";
                        foreach (var child in optionsArea.Children)
                            if (child is Button button) button.IsEnabled = false;
                        round++;
                        Task.Delay(750).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(ShowRound));
                    });
                    answer.TextColor = Ink;
                    optionsArea.Children.Add(answer);
                }
            }));
        }
        ShowRound();
    }

    void PlayPairs()
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
            var card = new Button { Text = "?", FontSize = 28, FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromArgb("#EEEAFE"), TextColor = Purple, CornerRadius = 17 };
            card.Clicked += async (_, _) =>
            {
                if (busy || matched.Contains(index) || open.Contains(index)) return;
                card.Text = symbols[index]; open.Add(index);
                if (open.Count < 2) return;
                busy = true;
                await Task.Delay(650);
                var first = open[0]; var second = open[1]; open.Clear();
                if (symbols[first] == symbols[second])
                {
                    matched.Add(first); matched.Add(second); matches++; AddPoints(2);
                    cards[first].BackgroundColor = Color.FromArgb("#DFF4EC");
                    cards[second].BackgroundColor = Color.FromArgb("#DFF4EC");
                    status.Text = matches == 6 ? "Toutes les paires trouvées ! 🎉" : $"Paires trouvées : {matches} / 6";
                    if (matches == 6) AddPoints(8);
                }
                else { cards[first].Text = "?"; cards[second].Text = "?"; }
                busy = false;
            };
            cards[i] = card; grid.Add(card, i % 3, i / 3);
        }
        body.Children.Add(status);
        body.Children.Add(Panel(grid));
        body.Children.Add(MakeButton("Nouvelle partie", Purple, PlayPairs));
    }

    void PlayStars()
    {
        StartPage("Attrape les étoiles", "Tape la case avec l’étoile avant la fin du chrono !");
        taps = 0; seconds = 20; running = true;
        var timer = Text("20 secondes", 17, true, Muted);
        var score = Text("0 étoile", 20, true);
        var grid = new Grid { RowSpacing = 8, ColumnSpacing = 8, HeightRequest = 300 };
        for (var i = 0; i < 3; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        }
        var cells = new Button[9];
        var starCell = random.Next(9);
        for (var i = 0; i < 9; i++)
        {
            var cellIndex = i;
            var cell = new Button { Text = i == starCell ? "⭐" : "·", FontSize = 34,
                BackgroundColor = i == starCell ? Color.FromArgb("#EEEAFE") : Colors.White,
                TextColor = Purple, CornerRadius = 18 };
            cell.Clicked += (_, _) =>
            {
                if (!running || cellIndex != starCell) return;
                taps++; score.Text = $"{taps} étoile{(taps == 1 ? "" : "s")} !";
                var previous = starCell;
                do starCell = random.Next(9); while (starCell == previous);
                for (var j = 0; j < cells.Length; j++)
                {
                    cells[j].Text = j == starCell ? "⭐" : "·";
                    cells[j].BackgroundColor = j == starCell ? Color.FromArgb("#EEEAFE") : Colors.White;
                }
            };
            cells[i] = cell; grid.Add(cell, i % 3, i / 3);
        }
        body.Children.Add(timer); body.Children.Add(Panel(grid)); body.Children.Add(score);
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!running) return false;
            seconds--; timer.Text = $"{seconds} seconde{(seconds == 1 ? "" : "s")}";
            if (seconds > 0) return true;
            running = false; AddPoints(taps); score.Text = $"Terminé ! {taps} étoiles attrapées 🎉";
            foreach (var cell in cells) cell.IsEnabled = false;
            return false;
        });
    }

    void PlayDice()
    {
        StartPage("Lance le dé", "Lance le dé et essaie d’obtenir un six !");
        var face = Text("🎲", 100, true);
        var result = Text("À toi de jouer !", 20, true, Muted);
        body.Children.Add(Panel(new VerticalStackLayout { Spacing = 12, Padding = new Thickness(10, 22), Children = { face, result } }, Color.FromArgb("#F1EEFF")));
        body.Children.Add(MakeButton("Lancer le dé", Green, async () =>
        {
            result.Text = "Ça tourne…";
            for (var i = 0; i < 7; i++)
            {
                face.Text = new[] { "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" }[random.Next(6)];
                await Task.Delay(80);
            }
            var number = random.Next(1, 7);
            face.Text = new[] { "", "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" }[number];
            result.Text = number == 6 ? "Un six ! +3 points 🎉" : $"Tu as obtenu {number}. Encore ?";
            if (number == 6) AddPoints(3);
        }));
    }

    void PlayReflex()
    {
        StartPage("Réflexe", "Attends le vert… puis appuie vite !");
        armed = false; running = true;
        var status = Text("Patiente un instant…", 18, true, Muted);
        Button circle = null!;
        circle = MakeButton("🟠", Color.FromArgb("#ED7C55"), () =>
        {
            if (!running) return;
            if (!armed)
            {
                running = false; status.Text = "Trop tôt ! Essaie encore."; circle.Text = "🙈";
                body.Children.Add(MakeButton("Rejouer", Purple, PlayReflex)); return;
            }
            running = false;
            var milliseconds = (DateTime.UtcNow - startAt).TotalMilliseconds;
            var reward = Math.Max(1, 10 - (int)(milliseconds / 100)); AddPoints(reward);
            status.Text = $"{milliseconds:0} ms — +{reward} points !";
            body.Children.Add(MakeButton("Encore", Purple, PlayReflex));
        }, 230);
        circle.FontSize = 70;
        body.Children.Add(Panel(circle, Color.FromArgb("#FFF0EB")));
        body.Children.Add(status);
        Task.Delay(random.Next(1500, 4500)).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
        {
            if (!running) return;
            armed = true; startAt = DateTime.UtcNow; circle.Text = "🟢";
            circle.BackgroundColor = Green; status.Text = "MAINTENANT !";
        }));
    }

    void PlayBubbles()
    {
        StartPage("Éclate les bulles", "Tape les bulles colorées avant la fin du chrono !");
        taps = 0; seconds = 20; running = true;
        var timer = Text("20 secondes", 17, true, Muted); var score = Text("0 bulle", 20, true);
        var grid = new Grid { RowSpacing = 9, ColumnSpacing = 9, HeightRequest = 330 };
        for (var i = 0; i < 4; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        }
        var colors = new[] { "#72D6E8", "#F4A7D5", "#A69BFA", "#87DDAA", "#FFD477" };
        var bubbles = new Button[16];
        for (var i = 0; i < bubbles.Length; i++)
        {
            var bubble = new Button { Text = "●", FontSize = 31,
                TextColor = Color.FromArgb(colors[random.Next(colors.Length)]),
                BackgroundColor = Colors.White, CornerRadius = 22 };
            bubble.Clicked += (_, _) =>
            {
                if (!running) return;
                taps++; score.Text = $"{taps} bulles éclatées !"; bubble.Text = "✨";
                Task.Delay(160).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() => { if (running) bubble.Text = "●"; }));
            };
            bubbles[i] = bubble; grid.Add(bubble, i % 4, i / 4);
        }
        body.Children.Add(timer); body.Children.Add(Panel(grid)); body.Children.Add(score);
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!running) return false;
            seconds--; timer.Text = $"{seconds} seconde{(seconds == 1 ? "" : "s")}";
            if (seconds > 0) return true;
            running = false; AddPoints(taps); score.Text = $"Terminé ! {taps} bulles éclatées 🫧";
            foreach (var bubble in bubbles) bubble.IsEnabled = false;
            return false;
        });
    }

    void PlayCoin()
    {
        StartPage("Pile ou face", "Pas besoin de choisir : lance la pièce !");
        var coin = Text("🪙", 100, true); var result = Text("Pile… ou face ?", 22, true);
        body.Children.Add(Panel(new VerticalStackLayout { Spacing = 12, Padding = new Thickness(12, 20), Children = { coin, result } }, Color.FromArgb("#FFF7E4")));
        body.Children.Add(MakeButton("Lancer la pièce", Color.FromArgb("#E7A735"), async () =>
        {
            result.Text = "La pièce tourne…";
            for (var i = 0; i < 6; i++) { coin.Text = i % 2 == 0 ? "🟡" : "⚪"; await Task.Delay(100); }
            var pile = random.Next(2) == 0; coin.Text = pile ? "🟡" : "⚪"; result.Text = pile ? "PILE !" : "FACE !";
        }));
    }

    void PlaySurprise()
    {
        StartPage("Machine surprise", "Appuie et découvre ton emoji porte-bonheur !");
        var emojis = new[] { "🐱", "🍕", "🚀", "🦄", "🍀", "🎈", "🐸", "🌈", "🍉" };
        var display = Text("🎰", 90, true); var message = Text("Qui va apparaître ?", 20, true);
        body.Children.Add(Panel(new VerticalStackLayout { Spacing = 12, Padding = new Thickness(12, 20), Children = { display, message } }, Color.FromArgb("#FCEAF4")));
        body.Children.Add(MakeButton("Surprise !", Color.FromArgb("#DE5E91"), async () =>
        {
            message.Text = "Roulement…";
            for (var i = 0; i < 8; i++) { display.Text = emojis[random.Next(emojis.Length)]; await Task.Delay(90); }
            display.Text = emojis[random.Next(emojis.Length)]; message.Text = "Encore ?";
        }));
    }

    void AddPoints(int amount)
    {
        points += amount;
        Preferences.Default.Set("points", points);
    }
}
