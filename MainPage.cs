namespace Kaskrout;

public class MainPage : ContentPage
{
    readonly Color ink = Color.FromArgb("#18233A");
    readonly Color muted = Color.FromArgb("#68738A");
    readonly Color paper = Color.FromArgb("#F4F6FB");
    readonly VerticalStackLayout body = new() { Spacing = 16, Padding = new Thickness(20, 22) };
    int points;
    int taps;
    int seconds;
    bool running;
    bool armed;
    DateTime startAt;
    readonly Random random = new();
    Label scoreLabel = new();

    public MainPage()
    {
        Title = "Kaskrout";
        BackgroundColor = paper;
        var scroll = new ScrollView { Content = body };
        Content = scroll;
        ShowHome();
    }

    Label Text(string value, double size, bool bold = false, Color? color = null) => new()
    {
        Text = value, FontSize = size, FontAttributes = bold ? FontAttributes.Bold : FontAttributes.None,
        TextColor = color ?? ink, HorizontalTextAlignment = TextAlignment.Center
    };

    Button MakeButton(string label, Color color, Action action)
    {
        var b = new Button { Text = label, BackgroundColor = color, TextColor = Colors.White,
            CornerRadius = 20, HeightRequest = 58, FontSize = 18, FontAttributes = FontAttributes.Bold };
        b.Clicked += (_, _) => action();
        return b;
    }

    void Reset(string title, string subtitle)
    {
        body.Children.Clear();
        body.Children.Add(new Button { Text = "← Accueil", HorizontalOptions = LayoutOptions.Start,
            BackgroundColor = Colors.Transparent, TextColor = muted, FontSize = 16, Padding = 0 });
        ((Button)body.Children[^1]).Clicked += (_, _) => ShowHome();
        body.Children.Add(Text(title, 30, true));
        body.Children.Add(Text(subtitle, 15, false, muted));
    }

    void ShowHome()
    {
        body.Children.Clear();
        body.Children.Add(Text("PAUSE JEUX", 14, true, Color.FromArgb("#6959D9")));
        body.Children.Add(Text("Tu as 2 minutes ?", 32, true));
        body.Children.Add(Text("Choisis un jeu et amuse-toi. Aucune règle compliquée.", 16, false, muted));
        var score = Text($"⭐  {points} points", 18, true, Color.FromArgb("#D68A13"));
        scoreLabel = score; body.Children.Add(score);
        body.Children.Add(GameCard("🎯", "Attrape les étoiles", "Tape le plus d’étoiles en 20 secondes.", Color.FromArgb("#6658D3"), PlayStars));
        body.Children.Add(GameCard("⚡", "Réflexe", "Appuie dès que le cercle devient vert.", Color.FromArgb("#ED7C55"), PlayReflex));
        body.Children.Add(GameCard("🎲", "Lance le dé", "Lance autant de fois que tu veux.", Color.FromArgb("#2DA889"), PlayDice));
        body.Children.Add(GameCard("🪙", "Pile ou face", "Laisse le hasard choisir.", Color.FromArgb("#E7A735"), PlayCoin));
    }

    View GameCard(string emoji, string title, string detail, Color color, Action action)
    {
        var button = new Button { Text = $"{emoji}   {title}\n{detail}",
            BackgroundColor = Colors.White, TextColor = ink, CornerRadius = 22, HeightRequest = 90,
            FontSize = 17, HorizontalTextAlignment = TextAlignment.Start, Padding = new Thickness(18, 10) };
        button.Clicked += (_, _) => action();
        return button;
    }

    void PlayStars()
    {
        Reset("Attrape les étoiles", "Tape l’étoile avant qu’elle ne change de place !");
        taps = 0; seconds = 20; running = true;
        var timer = Text("20 secondes", 18, true, muted); body.Children.Add(timer);
        Button target = null!;
        target = MakeButton("⭐", Color.FromArgb("#6658D3"), () =>
        {
            if (!running) return;
            taps++; target.Text = new[] { "⭐", "🌟", "✨", "💫" }[random.Next(4)];
            target.ScaleTo(1.2, 70).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() => target.ScaleTo(1, 70)));
            result.Text = $"{taps} étoiles !";
        });
        target.FontSize = 56; target.HeightRequest = 180; target.CornerRadius = 90;
        var result = Text("À toi de jouer !", 20, true);
        body.Children.Add(target); body.Children.Add(result);
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!running) return false;
            seconds--; timer.Text = $"{seconds} seconde{(seconds > 1 ? "s" : "")}";
            if (seconds > 0) return true;
            running = false; points += taps; scoreLabel.Text = $"⭐  {points} points";
            result.Text = $"Terminé ! Tu as attrapé {taps} étoiles. 🎉";
            target.IsEnabled = false; return false;
        });
    }

    void PlayReflex()
    {
        Reset("Réflexe", "Attends le vert… puis appuie vite !");
        var circle = MakeButton("🟠", Color.FromArgb("#ED7C55"), () =>
        {
            if (!armed) return;
            var ms = (DateTime.UtcNow - startAt).TotalMilliseconds;
            running = false; points += Math.Max(1, 10 - (int)(ms / 100));
            body.Children.Add(Text($"{ms:0} ms — bien joué !", 23, true, Color.FromArgb("#2DA889")));
            body.Children.Add(MakeButton("Encore", Color.FromArgb("#6658D3"), PlayReflex));
        });
        circle.HeightRequest = 240; circle.FontSize = 70; body.Children.Add(circle);
        var status = Text("Patiente un instant…", 18, true, muted); body.Children.Add(status);
        armed = false; running = true;
        var delay = random.Next(1500, 4500);
        Task.Delay(delay).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
        {
            if (!running) return;
            armed = true; startAt = DateTime.UtcNow; circle.Text = "🟢";
            circle.BackgroundColor = Color.FromArgb("#2DA889"); status.Text = "MAINTENANT !";
        }));
    }

    void PlayDice()
    {
        Reset("Lance le dé", "Appuie quand tu veux. Le hasard décide !");
        var face = Text("🎲", 100, true); body.Children.Add(face);
        var result = Text("Prêt ?", 22, true); body.Children.Add(result);
        body.Children.Add(MakeButton("Lancer le dé", Color.FromArgb("#2DA889"), () =>
        {
            var n = random.Next(1, 7); face.Text = new[] { "", "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" }[n];
            result.Text = $"Tu as obtenu {n} !";
        }));
    }

    void PlayCoin()
    {
        Reset("Pile ou face", "Pas besoin de choisir : lance la pièce !");
        var coin = Text("🪙", 100, true); body.Children.Add(coin);
        var result = Text("Pile… ou face ?", 22, true); body.Children.Add(result);
        body.Children.Add(MakeButton("Lancer", Color.FromArgb("#E7A735"), () =>
        {
            var pile = random.Next(2) == 0; coin.Text = pile ? "🟡" : "⚪";
            result.Text = pile ? "PILE !" : "FACE !";
        }));
    }
}
