namespace Kaskrout;

public class MainPage : ContentPage
{
    static readonly Color Ink = Color.FromArgb("#18233A");
    static readonly Color Muted = Color.FromArgb("#68738A");
    static readonly Color Paper = Color.FromArgb("#F4F6FB");
    static readonly Color Purple = Color.FromArgb("#6658D3");
    static readonly Color Green = Color.FromArgb("#2DA889");
    readonly VerticalStackLayout body = new() { Spacing = 16, Padding = new Thickness(20, 22) };
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
        Text = value,
        FontSize = size,
        FontAttributes = bold ? FontAttributes.Bold : FontAttributes.None,
        TextColor = color ?? Ink,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center
    };

    Button MakeButton(string label, Color color, Action action, double height = 58)
    {
        var button = new Button
        {
            Text = label,
            BackgroundColor = color,
            TextColor = Colors.White,
            CornerRadius = 20,
            HeightRequest = height,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold
        };
        button.Clicked += (_, _) => action();
        return button;
    }

    void AddHomeButton()
    {
        var home = new Button
        {
            Text = "← Accueil", HorizontalOptions = LayoutOptions.Start,
            BackgroundColor = Colors.Transparent, TextColor = Muted,
            FontSize = 16, Padding = 0
        };
        home.Clicked += (_, _) => { running = false; ShowHome(); };
        body.Children.Add(home);
    }

    void StartPage(string title, string subtitle)
    {
        running = false;
        body.Children.Clear();
        AddHomeButton();
        body.Children.Add(Text(title, 30, true));
        body.Children.Add(Text(subtitle, 15, false, Muted));
    }

    void ShowHome()
    {
        running = false;
        body.Children.Clear();
        body.Children.Add(Text("KASKROUT", 14, true, Purple));
        body.Children.Add(Text("Tu as 2 minutes ?", 32, true));
        body.Children.Add(Text("Choisis un jeu et amuse-toi. Aucune règle compliquée.", 16, false, Muted));
        body.Children.Add(Text($"⭐  {points} points", 18, true, Color.FromArgb("#D68A13")));
        body.Children.Add(GameCard("🎯", "Attrape les étoiles", "Tape les étoiles en 20 secondes.", Purple, PlayStars));
        body.Children.Add(GameCard("⚡", "Réflexe", "Appuie dès que le cercle devient vert.", Color.FromArgb("#ED7C55"), PlayReflex));
        body.Children.Add(GameCard("🎲", "Lance le dé", "Lance autant de fois que tu veux.", Green, PlayDice));
        body.Children.Add(GameCard("🪙", "Pile ou face", "Laisse le hasard choisir.", Color.FromArgb("#E7A735"), PlayCoin));
        body.Children.Add(GameCard("🎰", "Machine surprise", "Appuie et découvre ton emoji porte-bonheur.", Color.FromArgb("#DE5E91"), PlaySurprise));
        body.Children.Add(GameCard("🫧", "Éclate les bulles", "Éclate les bulles avant la fin du chrono.", Color.FromArgb("#38A9CC"), PlayBubbles));
    }

    View GameCard(string emoji, string title, string detail, Color color, Action action)
    {
        var button = new Button
        {
            Text = $"{emoji}   {title}\n{detail}", BackgroundColor = Colors.White,
            TextColor = Ink, CornerRadius = 22, HeightRequest = 90,
            FontSize = 17, HorizontalTextAlignment = TextAlignment.Start,
            Padding = new Thickness(18, 10), BorderColor = color, BorderWidth = 1
        };
        button.Clicked += (_, _) => action();
        return button;
    }

    void PlayStars()
    {
        StartPage("Attrape les étoiles", "Tape la case avec l’étoile avant qu’elle ne bouge !");
        taps = 0;
        seconds = 20;
        running = true;
        var timer = Text("20 secondes", 18, true, Muted);
        var score = Text("0 étoile", 20, true);
        var grid = new Grid { RowSpacing = 8, ColumnSpacing = 8, HeightRequest = 300 };
        for (var i = 0; i < 3; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }
        var cells = new Button[9];
        var starCell = random.Next(9);
        for (var i = 0; i < 9; i++)
        {
            var cellIndex = i;
            var cell = new Button
            {
                Text = i == starCell ? "⭐" : "·", FontSize = 34,
                BackgroundColor = i == starCell ? Color.FromArgb("#EEEAFE") : Colors.White,
                TextColor = Purple, CornerRadius = 18
            };
            cell.Clicked += (_, _) =>
            {
                if (!running) return;
                if (cellIndex == starCell)
                {
                    taps++;
                    score.Text = $"{taps} étoile{(taps == 1 ? "" : "s")} !";
                    starCell = random.Next(9);
                    for (var j = 0; j < cells.Length; j++)
                    {
                        cells[j].Text = j == starCell ? "⭐" : "·";
                        cells[j].BackgroundColor = j == starCell ? Color.FromArgb("#EEEAFE") : Colors.White;
                    }
                }
            };
            cells[i] = cell;
            grid.Add(cell, i % 3, i / 3);
        }
        body.Children.Add(timer);
        body.Children.Add(grid);
        body.Children.Add(score);
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!running) return false;
            seconds--;
            timer.Text = $"{seconds} seconde{(seconds == 1 ? "" : "s")}";
            if (seconds > 0) return true;
            running = false;
            AddPoints(taps);
            score.Text = $"Terminé ! {taps} étoiles attrapées 🎉";
            foreach (var cell in cells) cell.IsEnabled = false;
            return false;
        });
    }

    void PlayReflex()
    {
        StartPage("Réflexe", "Attends le vert… puis appuie vite !");
        armed = false;
        running = true;
        var status = Text("Patiente un instant…", 18, true, Muted);
        var circle = MakeButton("🟠", Color.FromArgb("#ED7C55"), () =>
        {
            if (!running) return;
            if (!armed)
            {
                running = false;
                status.Text = "Trop tôt ! Essaie encore.";
                circle.Text = "🙈";
                body.Children.Add(MakeButton("Rejouer", Purple, PlayReflex));
                return;
            }
            running = false;
            var milliseconds = (DateTime.UtcNow - startAt).TotalMilliseconds;
            var reward = Math.Max(1, 10 - (int)(milliseconds / 100));
            AddPoints(reward);
            status.Text = $"{milliseconds:0} ms — +{reward} points !";
            body.Children.Add(MakeButton("Encore", Purple, PlayReflex));
        }, 240);
        circle.FontSize = 70;
        body.Children.Add(circle);
        body.Children.Add(status);
        Task.Delay(random.Next(1500, 4500)).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
        {
            if (!running) return;
            armed = true;
            startAt = DateTime.UtcNow;
            circle.Text = "🟢";
            circle.BackgroundColor = Green;
            status.Text = "MAINTENANT !";
        }));
    }

    void PlayDice()
    {
        StartPage("Lance le dé", "Appuie quand tu veux. Le hasard décide !");
        var face = Text("🎲", 100, true);
        var result = Text("Prêt ?", 22, true);
        body.Children.Add(face);
        body.Children.Add(result);
        body.Children.Add(MakeButton("Lancer le dé", Green, () =>
        {
            var number = random.Next(1, 7);
            face.Text = new[] { "", "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" }[number];
            result.Text = $"Tu as obtenu {number} !";
            if (number == 6) AddPoints(1);
        }));
    }

    void PlayCoin()
    {
        StartPage("Pile ou face", "Pas besoin de choisir : lance la pièce !");
        var coin = Text("🪙", 100, true);
        var result = Text("Pile… ou face ?", 22, true);
        body.Children.Add(coin);
        body.Children.Add(result);
        body.Children.Add(MakeButton("Lancer", Color.FromArgb("#E7A735"), () =>
        {
            var pile = random.Next(2) == 0;
            coin.Text = pile ? "🟡" : "⚪";
            result.Text = pile ? "PILE !" : "FACE !";
        }));
    }

    void PlaySurprise()
    {
        StartPage("Machine surprise", "Appuie et découvre ton emoji porte-bonheur !");
        var emojis = new[] { "🐱", "🍕", "🚀", "🦄", "🍀", "🎈", "🐸", "🌈", "🍉" };
        var surprise = Text("🎰", 100, true);
        var message = Text("Qui va apparaître ?", 22, true);
        body.Children.Add(surprise);
        body.Children.Add(message);
        body.Children.Add(MakeButton("Surprise !", Color.FromArgb("#DE5E91"), () =>
        {
            surprise.Text = emojis[random.Next(emojis.Length)];
            message.Text = "Encore ?";
        }));
    }

    void PlayBubbles()
    {
        StartPage("Éclate les bulles", "Tape les bulles colorées avant la fin du chrono !");
        taps = 0;
        seconds = 20;
        running = true;
        var timer = Text("20 secondes", 18, true, Muted);
        var score = Text("0 bulle", 20, true);
        var bubbleGrid = new Grid { RowSpacing = 10, ColumnSpacing = 10, HeightRequest = 330 };
        for (var i = 0; i < 4; i++)
        {
            bubbleGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            bubbleGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }
        var colors = new[] { "#72D6E8", "#F4A7D5", "#A69BFA", "#87DDAA", "#FFD477" };
        for (var i = 0; i < 16; i++)
        {
            var bubble = new Button
            {
                Text = "●", FontSize = 32, TextColor = Color.FromArgb(colors[random.Next(colors.Length)]),
                BackgroundColor = Colors.White, CornerRadius = 24
            };
            bubble.Clicked += (_, _) =>
            {
                if (!running) return;
                taps++;
                score.Text = $"{taps} bulles éclatées !";
                bubble.Text = "✨";
                bubble.TextColor = Color.FromArgb(colors[random.Next(colors.Length)]);
                Task.Delay(180).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (running) bubble.Text = "●";
                }));
            };
            bubbleGrid.Add(bubble, i % 4, i / 4);
        }
        body.Children.Add(timer);
        body.Children.Add(bubbleGrid);
        body.Children.Add(score);
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!running) return false;
            seconds--;
            timer.Text = $"{seconds} seconde{(seconds == 1 ? "" : "s")}";
            if (seconds > 0) return true;
            running = false;
            AddPoints(taps);
            score.Text = $"Terminé ! {taps} bulles éclatées 🫧";
            foreach (var view in bubbleGrid.Children) view.IsEnabled = false;
            return false;
        });
    }

    void AddPoints(int amount)
    {
        points += amount;
        Preferences.Default.Set("points", points);
    }
}
