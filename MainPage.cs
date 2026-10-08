using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Kaskrout;

public class MainPage : ContentPage
{
    readonly string language;
    static readonly Dictionary<string, string[]> Translations = new()
    {
        ["Une petite pause ?"] = ["A little break?", "Even pauze?"] ,
        ["Choisis un mini-défi et amuse-toi !"] = ["Pick a mini challenge and have fun!", "Kies een mini-uitdaging en veel plezier!"],
        ["points"] = ["points", "punten"], ["CHOISIS TON JEU"] = ["CHOOSE A GAME", "KIES EEN SPEL"],
        ["Puzzle"] = ["Puzzle", "Puzzel"], ["Remets les tuiles en ordre"] = ["Put the tiles in order", "Zet de tegels op volgorde"],
        ["Objet manquant"] = ["Missing object", "Ontbrekend voorwerp"], ["Observe, puis retrouve-le"] = ["Look, then find what's missing", "Kijk goed en vind wat ontbreekt"],
        ["Paires"] = ["Matching pairs", "Paren"], ["Associe les images identiques"] = ["Match the identical pictures", "Zoek de gelijke plaatjes"],
        ["Solitaire"] = ["Solitaire", "Patience"], ["Jeu de cartes classique"] = ["Classic card game", "Klassiek kaartspel"],
        ["Touche la pioche pour tirer. Range les cartes par couleur, de l’as au roi."] = ["Tap the stock to draw. Build each suit from ace to king.", "Tik op de stapel om te trekken. Leg elke kleur van aas tot koning."],
        ["Coups : 0"] = ["Moves: 0", "Zetten: 0"], ["Déplacement impossible."] = ["That move is not allowed.", "Deze zet is niet toegestaan."],
        ["Partie terminée ! Tu as gagné 🎉"] = ["Game complete! You won 🎉", "Spel voltooid! Je hebt gewonnen 🎉"],
        ["Pioche vide. Touche-la pour reprendre les cartes."] = ["Stock empty. Tap it to recycle the cards.", "Stapel leeg. Tik erop om de kaarten terug te nemen."],
        ["Choisis une carte, puis sa destination."] = ["Select a card, then its destination.", "Kies een kaart en daarna de bestemming."],
        ["Rangée vide"] = ["Empty column", "Lege kolom"],
        ["SCORE"] = ["SCORE", "SCORE"], ["TEMPS"] = ["TIME", "TIJD"], ["COUPS"] = ["MOVES", "ZETTEN"],
        ["Annuler"] = ["Undo", "Ongedaan maken"], ["Indice"] = ["Hint", "Hint"], ["Nouvelle partie"] = ["New game", "Nieuw spel"],
        ["Tableau vert, une carte à la fois."] = ["Green felt, one card at a time.", "Groen speelkleed, één kaart tegelijk."],
        ["Aucun coup évident. Pioche ou retourne une carte cachée."] = ["No obvious move. Draw or reveal a hidden card.", "Geen duidelijke zet. Trek een kaart of draai een verborgen kaart om."],
        ["Lance le dé"] = ["Roll the dice", "Gooi de dobbelsteen"], ["Un lancer porte-bonheur ?"] = ["A lucky roll?", "Een gelukkige worp?"],
        ["Réflexe"] = ["Reflex", "Reflex"], ["Attends le vert et appuie"] = ["Wait for green, then tap", "Wacht op groen en tik"],
        ["XP Minesweeper Classic"] = ["XP Minesweeper Classic", "XP Minesweeper Classic"],
        ["Démineur"] = ["Minesweeper", "Mijnenveger"], ["Repère les cases sûres"] = ["Spot the safe squares", "Zoek de veilige vakjes"],
        ["Repère les cases sûres et évite les mines."] = ["Find the safe squares and avoid the mines.", "Zoek de veilige vakjes en vermijd de mijnen."],
        ["Trouve les cases sûres"] = ["Find the safe squares", "Vind de veilige vakjes"],
        ["Découvre toutes les cases sans mine. Active le mode drapeau pour signaler un danger."] = ["Reveal every square without a mine. Turn on flag mode to mark a danger.", "Onthul alle vakjes zonder mijn. Zet de vlagmodus aan om gevaar aan te geven."],
        ["Drapeaux : désactivés"] = ["Flags: off", "Vlaggen: uit"], ["Drapeaux : activés"] = ["Flags: on", "Vlaggen: aan"],
        ["Première case sûre. À toi de jouer !"] = ["First square is safe. Your turn!", "Eerste vakje is veilig. Jij bent aan de beurt!"],
        ["Mines révélées ! Recommence pour tenter ta chance."] = ["Mine revealed! Start a new game and try again.", "Mijn gevonden! Start een nieuw spel en probeer opnieuw."],
        ["Grille nettoyée ! +10 points 🎉"] = ["Board cleared! +10 points 🎉", "Bord leeggemaakt! +10 punten 🎉"],
        ["Pile ou face"] = ["Heads or tails", "Kop of munt"], ["La pièce choisit pour toi"] = ["Let the coin choose for you", "Laat de munt voor je kiezen"],
        ["Puzzle coulissant"] = ["Sliding puzzle", "Schuifpuzzel"], ["Fais glisser les nombres pour les ranger de 1 à 8."] = ["Slide the numbers to put them in order from 1 to 8.", "Schuif de cijfers op volgorde van 1 tot 8."],
        ["Coups : 0"] = ["Moves: 0", "Zetten: 0"], ["Mélanger"] = ["Shuffle", "Schudden"],
        ["Observe les images, puis retrouve celle qui a disparu."] = ["Study the pictures, then find the one that disappeared.", "Bekijk de plaatjes en vind daarna wat verdwenen is."],
        ["Bien joué !"] = ["Well done!", "Goed gedaan!"], ["Mémorise bien…"] = ["Memorize them…", "Onthoud ze goed…"], ["Lequel a disparu ?"] = ["Which one is missing?", "Welke ontbreekt?"], ["Exactement ! ✨"] = ["That's right! ✨", "Precies! ✨"],
        ["Rejouer"] = ["Play again", "Opnieuw spelen"], ["Jeu des paires"] = ["Matching pairs", "Paren zoeken"], ["Retourne deux cartes et retrouve les images identiques."] = ["Flip two cards and find matching pictures.", "Draai twee kaarten om en zoek dezelfde plaatjes."],
        ["Paires trouvées : 0 / 6"] = ["Pairs found: 0 / 6", "Paren gevonden: 0 / 6"], ["Toutes les paires trouvées ! 🎉"] = ["All pairs found! 🎉", "Alle paren gevonden! 🎉"], ["Nouvelle partie"] = ["New game", "Nieuw spel"],
        ["Tape la case avec l’étoile avant la fin du chrono !"] = ["Tap the square with the star before time runs out!", "Tik op het vakje met de ster voordat de tijd om is!"],
        ["Lance le dé et essaie d’obtenir un six !"] = ["Roll the dice and try to get a six!", "Gooi de dobbelsteen en probeer zes te halen!"], ["À toi de jouer !"] = ["Your turn!", "Jij bent aan de beurt!"], ["Ça tourne…"] = ["Rolling…", "Hij rolt…"],
        ["Attends le vert… puis appuie vite !"] = ["Wait for green… then tap quickly!", "Wacht op groen… en tik dan snel!"], ["Patiente un instant…"] = ["Wait a moment…", "Wacht even…"], ["Trop tôt ! Essaie encore."] = ["Too soon! Try again.", "Te vroeg! Probeer opnieuw."], ["MAINTENANT !"] = ["NOW!", "NU!"], ["Encore"] = ["Again", "Nog een keer"],
        ["Pas besoin de choisir : lance la pièce !"] = ["No need to choose: flip the coin!", "Je hoeft niet te kiezen: gooi de munt op!"], ["Pile… ou face ?"] = ["Heads… or tails?", "Kop… of munt?"], ["Lancer la pièce"] = ["Flip the coin", "Gooi de munt"], ["La pièce tourne…"] = ["The coin is spinning…", "De munt draait…"], ["PILE !"] = ["HEADS!", "KOP!"], ["FACE !"] = ["TAILS!", "MUNT!"],
        ["seconde"] = ["second", "seconde"], ["secondes"] = ["seconds", "seconden"], ["étoile"] = ["star", "ster"], ["étoiles"] = ["stars", "sterren"],
        ["Terminé !"] = ["Time's up!", "Tijd is om!"], ["Tu as trouvé"] = ["You found", "Je vond"], ["objets sur"] = ["objects out of", "voorwerpen van"], ["C’était"] = ["It was", "Het was"], ["Bravo ! Puzzle terminé en"] = ["Great! Puzzle completed in", "Goed gedaan! Puzzel opgelost in"], ["coups"] = ["moves", "zetten"], ["Tu as obtenu"] = ["You rolled", "Je gooide"],
        ["étoile(s) !"] = ["star(s)!", "ster(ren)!"], ["étoiles attrapées"] = ["stars caught", "sterren gevangen"],
        ["Langue : Français"] = ["Language: English", "Taal: Nederlands"],
    };
    static readonly Color Ink = Color.FromArgb("#202743");
    static readonly Color Muted = Color.FromArgb("#7D849B");
    static readonly Color Paper = Color.FromArgb("#F7F6FC");
    static readonly Color Purple = Color.FromArgb("#715CE8");
    static readonly Color Green = Color.FromArgb("#31A98B");
    enum SolitaireSource { None, Waste, Tableau }
    sealed class SolitaireCard(int rank, char suit)
    {
        public int Rank { get; } = rank;
        public char Suit { get; } = suit;
        public bool FaceDown { get; set; }
    }
    sealed record SolitaireSnapshot(
        List<SolitaireCard> Stock, List<SolitaireCard> Waste,
        List<SolitaireCard>[] Tableau, Dictionary<char, List<SolitaireCard>> Foundations,
        int Score, int Moves);
    readonly VerticalStackLayout body = new() { Spacing = 16, Padding = new Thickness(20, 18, 20, 28) };
    readonly Random random = new();
    int points = Preferences.Default.Get("points", 0);
    bool running;
    bool armed;
    bool solitaireActive;
    Border? activeFeedback;
    DateTime startAt;

    public MainPage(string language = "fr")
    {
        this.language = language;
        Title = "Kaskrout";
        BackgroundColor = Paper;
        Content = new ScrollView { Content = body };
        ShowHome();
    }

    string T(string value) => language switch
    {
        "en" when Translations.TryGetValue(value, out var english) => english[0],
        "nl" when Translations.TryGetValue(value, out var dutch) => dutch[1],
        _ => value
    };

    string F(string french, string english, string dutch) => language switch
    {
        "en" => english,
        "nl" => dutch,
        _ => french
    };

    Label Text(string value, double size, bool bold = false, Color? color = null) => new()
    {
        Text = T(value), FontSize = size,
        FontAttributes = bold ? FontAttributes.Bold : FontAttributes.None,
        TextColor = color ?? Ink, HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center
    };

    Button MakeButton(string label, Color color, Action action, double height = 58)
    {
        var button = new Button
        {
            Text = T(label), BackgroundColor = color, TextColor = Colors.White,
            CornerRadius = 18, HeightRequest = height, FontSize = 18,
            FontAttributes = FontAttributes.Bold
        };
        button.Clicked += (_, _) => action();
        return button;
    }

    void ShowHome()
    {
        running = false;
        solitaireActive = false;
        activeFeedback = null;
        BackgroundColor = Paper;
        body.BackgroundColor = Colors.Transparent;
        body.Spacing = 16;
        body.Padding = new Thickness(20, 18, 20, 28);
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
            Content = Text(F($"⭐  {points} points", $"⭐  {points} points", $"⭐  {points} punten"), 17, true, Color.FromArgb("#BA841C"))
        };
        body.Children.Add(scorePill);
        body.Children.Add(MakeButton(F("🌐  Langue : Français", "🌐  Language: English", "🌐  Taal: Nederlands"), Purple, SelectLanguage, 46));
        body.Children.Add(Text("CHOISIS TON JEU", 13, true, Muted));

        var games = new (string Icon, string Title, string Detail, string Color, Action Play)[]
        {
            ("🧩", "Puzzle", "Remets les tuiles en ordre", "#EEEAFE", PlayPuzzle),
            ("👀", "Objet manquant", "Observe, puis retrouve-le", "#E4F5F1", PlayMissingObject),
            ("🃏", "Paires", "Associe les images identiques", "#FFF0E4", PlayPairs),
            ("🃏", "Solitaire", "Jeu de cartes classique", "#FFF7D9", PlaySolitaire),
            ("🎲", "Lance le dé", "Un lancer porte-bonheur ?", "#E8F3FF", PlayDice),
            ("⚡", "Réflexe", "Attends le vert et appuie", "#FFE9EC", PlayReflex),
            ("💎", "Démineur", "Repère les cases sûres", "#E8EEF5", PlayMinesweeper),
            ("🪙", "Pile ou face", "La pièce choisit pour toi", "#FFF2D3", PlayCoin)
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
                Padding = new Thickness(14), HeightRequest = 168,
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
            var tap = new TapGestureRecognizer();
            tap.Tapped += async (_, _) =>
            {
                await card.ScaleToAsync(0.96, 70);
                await card.ScaleToAsync(1, 110, Easing.SpringOut);
                game.Play();
            };
            card.GestureRecognizers.Add(tap);
            grid.Add(card, i % 2, i / 2);
        }
        body.Children.Add(grid);
    }

    void StartPage(string title, string subtitle)
    {
        running = false;
        solitaireActive = false;
        activeFeedback = null;
        BackgroundColor = Paper;
        body.BackgroundColor = Colors.Transparent;
        body.Spacing = 16;
        body.Padding = new Thickness(20, 18, 20, 28);
        body.Children.Clear();
        var top = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
        var back = new Button
        {
            Text = "←", FontSize = 22, TextColor = Purple, BackgroundColor = Colors.White,
            CornerRadius = 17, WidthRequest = 48, HeightRequest = 48, Padding = 0,
            Shadow = new Shadow { Brush = Color.FromArgb("#25715CE8"), Offset = new Point(0, 3), Radius = 7, Opacity = 0.35f }
        };
        back.Clicked += (_, _) => ShowHome();
        top.Add(back, 0, 0);
        top.Add(Text("KASKROUT", 12, true, Muted), 1, 0);
        var badge = new Border { BackgroundColor = Color.FromArgb("#E6F5F0"), StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 14 }, Padding = new Thickness(10, 5),
            Content = Text("JEU", 10, true, Green) };
        top.Add(badge, 2, 0);
        body.Children.Add(top);
        var icon = title switch
        {
            "Puzzle coulissant" => "🧩", "Objet manquant" => "🔎", "Jeu des paires" => "🃏",
            "Lance le dé" => "🎲", "Réflexe" => "⚡", "Démineur" => "💎",
            "Pile ou face" => "🪙", _ => "✨"
        };
        var heroGrid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 14
        };
        heroGrid.Add(new Border { WidthRequest = 62, HeightRequest = 62, BackgroundColor = Colors.White,
            StrokeThickness = 0, StrokeShape = new RoundRectangle { CornerRadius = 21 }, Content = Text(icon, 34) }, 0, 0);
        heroGrid.Add(new VerticalStackLayout { Spacing = 4, VerticalOptions = LayoutOptions.Center,
            Children = { Text(title, 22, true, Ink), Text(subtitle, 13, false, Muted) } }, 1, 0);
        var hero = new Border
        {
            Background = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0), EndPoint = new Point(1, 1),
                GradientStops = { new GradientStop(Color.FromArgb("#F0ECFF"), 0), new GradientStop(Color.FromArgb("#E3F5F1"), 1) }
            },
            StrokeThickness = 0, StrokeShape = new RoundRectangle { CornerRadius = 26 },
            Padding = new Thickness(17, 15),
            Shadow = new Shadow { Brush = Color.FromArgb("#28715CE8"), Offset = new Point(0, 5), Radius = 12, Opacity = 0.24f },
            Content = heroGrid
        };
        body.Children.Add(hero);
        hero.Opacity = 0; hero.TranslationY = 8;
        _ = hero.FadeToAsync(1, 220);
        _ = hero.TranslateToAsync(0, 0, 240, Easing.CubicOut);
    }

    void ShowGameFeedback(bool won, string message, bool temporary = false)
    {
        if (activeFeedback is not null && body.Children.Contains(activeFeedback))
            body.Children.Remove(activeFeedback);

        var accent = won ? Color.FromArgb("#8A5CE6") : Color.FromArgb("#D94D68");
        var gradient = new LinearGradientBrush { StartPoint = new Point(0, 0), EndPoint = new Point(1, 1) };
        gradient.GradientStops.Add(new GradientStop(Color.FromArgb(won ? "#FFF0BD" : "#FFE5EA"), 0));
        gradient.GradientStops.Add(new GradientStop(Color.FromArgb(won ? "#FBE0F1" : "#FFF0F2"), 1));
        var content = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 12
        };
        var icon = new Label { Text = won ? "🎉" : "😭", FontSize = 42, VerticalTextAlignment = TextAlignment.Center };
        var resultStack = new VerticalStackLayout
        {
            Spacing = 2, VerticalOptions = LayoutOptions.Center,
            Children = { Text(won ? F("Bravo !", "You did it!", "Goed gedaan!") : F("Oh non !", "Oh no!", "O nee!"), 19, true, accent), Text(message, 14, true, Ink) }
        };
        if (won) resultStack.Children.Add(Text("✨  🎊  ⭐  🎊  ✨", 14, true, Color.FromArgb("#CB8B22")));
        content.Add(icon, 0, 0);
        content.Add(resultStack, 1, 0);
        var card = new Border
        {
            Background = gradient,
            Stroke = won ? Color.FromArgb("#FFFFD166") : Color.FromArgb("#FFF1B9C4"),
            StrokeThickness = 1.5, StrokeShape = new RoundRectangle { CornerRadius = 23 },
            Padding = new Thickness(13, 10),
            Shadow = new Shadow { Brush = Color.FromArgb(won ? "#40D99B27" : "#25D94D68"), Offset = new Point(0, 4), Radius = 10, Opacity = 0.28f },
            Content = content
        };
        activeFeedback = card;
        body.Children.Insert(Math.Min(2, body.Children.Count), card);
        card.Scale = 0.82; card.Opacity = 0;
        _ = card.FadeToAsync(1, 250);
        _ = card.ScaleToAsync(1, 400, Easing.SpringOut);

        if (won)
        {
            _ = icon.RotateToAsync(14, 160);
            _ = icon.RotateToAsync(-14, 300, Easing.SpringOut);
            _ = icon.RotateToAsync(0, 180, Easing.SpringOut);
        }
        else
        {
            _ = icon.TranslateToAsync(-5, 0, 70);
            _ = icon.TranslateToAsync(5, 0, 90);
            _ = icon.TranslateToAsync(0, 0, 100);
        }

        if (temporary)
            Task.Delay(1350).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!ReferenceEquals(activeFeedback, card)) return;
                _ = card.FadeToAsync(0, 180);
                Task.Delay(190).ContinueWith(__ => MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (ReferenceEquals(activeFeedback, card)) body.Children.Remove(card);
                    if (ReferenceEquals(activeFeedback, card)) activeFeedback = null;
                }));
            }));
    }

    void SelectLanguage() => Application.Current!.MainPage = new LanguageSelectionPage();

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
                    Children = { Text("Bien joué !", 26, true, Green), Text(F($"Tu as trouvé {score} objets sur {rounds.Length}.", $"You found {score} objects out of {rounds.Length}.", $"Je vond {score} voorwerpen van de {rounds.Length}."), 18) }
                }));
                roundArea.Children.Add(MakeButton("Rejouer", Purple, PlayMissingObject));
                AddPoints(score * 3);
                ShowGameFeedback(true, F($"Défi terminé ! +{score * 3} points", $"Challenge complete! +{score * 3} points", $"Uitdaging voltooid! +{score * 3} punten"));
                return;
            }

            var items = rounds[round];
            var missing = items[random.Next(items.Length)];
            var visible = Text(string.Join("   ", items), 31);
            var question = Text("Mémorise bien…", 16, true, Purple);
        var panel = Panel(new VerticalStackLayout { Spacing = 14, Children = { visible, question } });
        roundArea.Children.Add(panel);
        panel.Scale = 0.96;
        _ = panel.ScaleToAsync(1, 220, Easing.SpringOut);
            var optionsArea = new Grid { RowSpacing = 9, ColumnSpacing = 9 };
            for (var i = 0; i < 2; i++)
            {
                optionsArea.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
                optionsArea.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            }
            roundArea.Children.Add(optionsArea);
            Task.Delay(1800).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!body.Children.Contains(roundArea) || !roundArea.Children.Contains(panel)) return;
                visible.Text = string.Join("   ", items.Where(x => x != missing));
                question.Text = T("Lequel a disparu ?");
                var decoys = new[] { "🍋", "🛴", "🐶", "🎁", "☀️", "🍪", "🚕", "🐻", "🎾", "🍇" }
                    .Where(x => !items.Contains(x)).OrderBy(_ => random.Next()).Take(3).ToList();
                var optionIndex = 0;
                foreach (var option in decoys.Append(missing).OrderBy(_ => random.Next()))
                {
                    var answer = MakeButton(option, Colors.White, () =>
                    {
                        if (optionsArea.Children.All(x => !x.IsEnabled)) return;
                        if (option == missing)
                        {
                            score++; question.Text = T("Exactement ! ✨");
                            ShowGameFeedback(true, F("Bonne réponse !", "Correct answer!", "Goed antwoord!"), true);
                        }
                        else
                        {
                            question.Text = F($"C’était {missing} !", $"It was {missing}!", $"Het was {missing}!");
                            ShowGameFeedback(false, F("Essaie encore au prochain tour.", "Try again next round.", "Probeer het opnieuw in de volgende ronde."), true);
                        }
                        foreach (var child in optionsArea.Children)
                            if (child is Button button) button.IsEnabled = false;
                        round++;
                        Task.Delay(750).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(ShowRound));
                    }, 74);
                    answer.FontSize = 35; answer.CornerRadius = 20;
                    answer.Shadow = new Shadow { Brush = Color.FromArgb("#24715CE8"), Offset = new Point(0, 3), Radius = 7, Opacity = 0.25f };
                    optionsArea.Add(answer, optionIndex % 2, optionIndex / 2);
                    optionIndex++;
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
            var card = new Button { Text = "✦", FontSize = 29, FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromArgb("#715CE8"), TextColor = Colors.White, CornerRadius = 20,
                Shadow = new Shadow { Brush = Color.FromArgb("#65715CE8"), Offset = new Point(0, 5), Radius = 9, Opacity = 0.34f } };
            card.Clicked += async (_, _) =>
            {
                if (busy || matched.Contains(index) || open.Contains(index)) return;
                await card.RotateYToAsync(90, 90);
                card.Text = symbols[index]; card.BackgroundColor = Colors.White;
                card.TextColor = Color.FromArgb("#D93D65");
                card.FontSize = 39; open.Add(index);
                await card.RotateYToAsync(0, 100);
                if (open.Count < 2) return;
                busy = true;
                await Task.Delay(650);
                var first = open[0]; var second = open[1]; open.Clear();
                if (symbols[first] == symbols[second])
                {
                    matched.Add(first); matched.Add(second); matches++; AddPoints(2);
                    cards[first].BackgroundColor = Color.FromArgb("#CFF5E7");
                    cards[second].BackgroundColor = Color.FromArgb("#CFF5E7");
                    await cards[first].ScaleToAsync(1.08, 100); await cards[first].ScaleToAsync(1, 160, Easing.SpringOut);
                    await cards[second].ScaleToAsync(1.08, 100); await cards[second].ScaleToAsync(1, 160, Easing.SpringOut);
                    status.Text = matches == 6 ? T("Toutes les paires trouvées ! 🎉") : F($"Paires trouvées : {matches} / 6", $"Pairs found: {matches} / 6", $"Paren gevonden: {matches} / 6");
                    if (matches == 6)
                    {
                        AddPoints(8);
                        ShowGameFeedback(true, F("Toutes les paires sont trouvées !", "You found every pair!", "Je hebt alle paren gevonden!"));
                    }
                }
                else
                {
                    ShowGameFeedback(false, F("Ces cartes ne vont pas ensemble.", "Those cards do not match.", "Deze kaarten horen niet bij elkaar."), true);
                    cards[first].Text = "✦"; cards[second].Text = "✦";
                    foreach (var missed in new[] { cards[first], cards[second] })
                    {
                        await missed.RotateYToAsync(90, 80);
                        missed.BackgroundColor = Color.FromArgb("#715CE8"); missed.TextColor = Colors.White; missed.FontSize = 29;
                        await missed.RotateYToAsync(0, 100);
                    }
                }
                busy = false;
            };
            cards[i] = card; grid.Add(card, i % 3, i / 3);
        }
        body.Children.Add(status);
        body.Children.Add(Panel(grid));
        body.Children.Add(MakeButton("Nouvelle partie", Purple, PlayPairs));
    }

    void PlaySolitaire()
    {
        running = false;
        solitaireActive = false;
        activeFeedback = null;
        body.Children.Clear();
        body.Spacing = 11;
        body.Padding = new Thickness(12, 12, 12, 18);
        BackgroundColor = Color.FromArgb("#0B5A3A");
        body.BackgroundColor = Color.FromArgb("#0B5A3A");

        var suits = new[] { '♠', '♥', '♦', '♣' };
        var deck = (from suit in suits from rank in Enumerable.Range(1, 13) select new SolitaireCard(rank, suit))
            .OrderBy(_ => random.Next()).ToList();
        var stock = new List<SolitaireCard>();
        var waste = new List<SolitaireCard>();
        var tableau = Enumerable.Range(0, 7).Select(_ => new List<SolitaireCard>()).ToArray();
        var foundations = suits.ToDictionary(suit => suit, _ => new List<SolitaireCard>());
        var moves = 0;
        var score = 0;
        var elapsedSeconds = 0;
        var source = SolitaireSource.None;
        var sourceColumn = -1;
        var sourceIndex = -1;
        var undoStack = new Stack<SolitaireSnapshot>();
        SolitaireCard? hintCard = null;
        SolitaireCard? justMovedCard = null;
        var hintTargetColumn = -1;
        char? hintFoundationSuit = null;
        var status = new Label
        {
            Text = T("Tableau vert, une carte à la fois."), FontSize = 13, TextColor = Color.FromArgb("#D7E8DE"),
            HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Fill
        };
        var tableauGrid = new Grid { ColumnSpacing = 3, RowSpacing = 0, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Start };
        tableauGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        for (var i = 0; i < 7; i++)
            tableauGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

        for (var column = 0; column < 7; column++)
        {
            for (var row = 0; row <= column; row++)
            {
                var card = deck[0];
                deck.RemoveAt(0);
                card.FaceDown = row != column;
                tableau[column].Add(card);
            }
        }
        stock.AddRange(deck);

        var scoreLabel = new Label { Text = "0", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = Colors.White, HorizontalTextAlignment = TextAlignment.Center };
        var timeLabel = new Label { Text = "00:00", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = Colors.White, HorizontalTextAlignment = TextAlignment.Center };
        var movesLabel = new Label { Text = "0", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = Colors.White, HorizontalTextAlignment = TextAlignment.Center };
        var header = new Grid { ColumnSpacing = 8, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
        var backButton = new Button { Text = "‹", FontSize = 30, Padding = 0, WidthRequest = 42, HeightRequest = 46,
            BackgroundColor = Color.FromArgb("#124C35"), TextColor = Colors.White, CornerRadius = 16 };
        backButton.Clicked += (_, _) => ShowHome();
        var title = new Label { Text = "SOLITAIRE", FontSize = 19, FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White, VerticalTextAlignment = TextAlignment.Center, CharacterSpacing = 1.5 };
        var restartTop = new Button { Text = "⟳", FontSize = 24, Padding = 0, WidthRequest = 42, HeightRequest = 46,
            BackgroundColor = Color.FromArgb("#124C35"), TextColor = Colors.White, CornerRadius = 16 };
        restartTop.Clicked += (_, _) => PlaySolitaire();
        header.Add(backButton, 0, 0); header.Add(title, 1, 0); header.Add(restartTop, 2, 0);

        Label StatName(string label) => new() { Text = T(label), FontSize = 10, FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#B8D5C4"), HorizontalTextAlignment = TextAlignment.Center };
        Border Stat(string name, Label value) => new()
        {
            BackgroundColor = Color.FromArgb("#124C35"), StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 15 }, Padding = new Thickness(7, 6),
            Content = new VerticalStackLayout { Spacing = 1, Children = { StatName(name), value } }
        };
        var stats = new Grid { ColumnSpacing = 8 };
        for (var i = 0; i < 3; i++) stats.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        stats.Add(Stat("SCORE", scoreLabel), 0, 0);
        stats.Add(Stat("TEMPS", timeLabel), 1, 0);
        stats.Add(Stat("COUPS", movesLabel), 2, 0);

        var topRow = new Grid { ColumnSpacing = 6, HeightRequest = 72 };
        for (var i = 0; i < 6; i++)
            topRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        Border Slot(char? suit, SolitaireCard? card, Action tap, bool highlighted = false)
        {
            var red = suit.HasValue && IsRedSuit(suit.Value);
            View content = card is null
                ? new Label { Text = suit?.ToString() ?? "·", FontSize = 25, FontAttributes = FontAttributes.Bold,
                    TextColor = suit.HasValue ? (red ? Color.FromArgb("#73BCA0") : Color.FromArgb("#92C2AB")) : Colors.White,
                    HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center }
                : CardFace(card, false, false);
            var slot = new Border
            {
                WidthRequest = 46, HeightRequest = 66, Padding = 2,
                BackgroundColor = card is null ? Color.FromArgb("#14563A") : Colors.White,
                Stroke = highlighted ? Color.FromArgb("#FFE16A") : Color.FromArgb("#91B29D"),
                StrokeThickness = highlighted ? 2.5 : 1.2,
                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                Content = content
            };
            if (card is not null) slot.Shadow = new Shadow { Brush = Color.FromArgb("#50001810"), Offset = new Point(0, 3), Radius = 4, Opacity = 0.42f };
            var gesture = new TapGestureRecognizer(); gesture.Tapped += (_, _) => tap(); slot.GestureRecognizers.Add(gesture);
            return slot;
        }

        var foundationSlots = new Border[4];
        Border wasteSlot = null!;
        Border stockSlot = null!;
        wasteSlot = Slot(null, null, () => SelectWaste());
        stockSlot = Slot(null, null, () => DrawStock());
        for (var i = 0; i < suits.Length; i++)
        {
            var suit = suits[i];
            foundationSlots[i] = Slot(suit, null, () => MoveToFoundation(suit));
            topRow.Add(foundationSlots[i], i, 0);
        }
        topRow.Add(wasteSlot, 4, 0); topRow.Add(stockSlot, 5, 0);

        body.Children.Add(header);
        body.Children.Add(stats);
        body.Children.Add(topRow);
        body.Children.Add(tableauGrid);
        body.Children.Add(status);

        void SaveUndo()
        {
            SolitaireCard Copy(SolitaireCard card) => new(card.Rank, card.Suit) { FaceDown = card.FaceDown };
            undoStack.Push(new SolitaireSnapshot(
                stock.Select(Copy).ToList(), waste.Select(Copy).ToList(),
                tableau.Select(pile => pile.Select(Copy).ToList()).ToArray(),
                foundations.ToDictionary(pair => pair.Key, pair => pair.Value.Select(Copy).ToList()), score, moves));
        }

        void Undo()
        {
            if (undoStack.Count == 0) { status.Text = T("Aucun coup à annuler."); return; }
            var previous = undoStack.Pop();
            stock.Clear(); stock.AddRange(previous.Stock);
            waste.Clear(); waste.AddRange(previous.Waste);
            for (var i = 0; i < tableau.Length; i++) { tableau[i].Clear(); tableau[i].AddRange(previous.Tableau[i]); }
            foreach (var suit in suits) { foundations[suit].Clear(); foundations[suit].AddRange(previous.Foundations[suit]); }
            score = previous.Score; moves = previous.Moves;
            source = SolitaireSource.None; hintCard = null; hintTargetColumn = -1; hintFoundationSuit = null;
            solitaireActive = true; status.Text = T("Coup annulé."); Refresh();
        }

        bool CanPlaceOn(SolitaireCard card, List<SolitaireCard> pile) => pile.Count == 0
            ? card.Rank == 13
            : !pile[^1].FaceDown && pile[^1].Rank == card.Rank + 1 && IsRedSuit(pile[^1].Suit) != IsRedSuit(card.Suit);

        void ShowHint()
        {
            hintCard = null; hintTargetColumn = -1; hintFoundationSuit = null;
            SolitaireCard? candidate = waste.Count > 0 ? waste[^1] : null;
            if (candidate is not null && CanGoToFoundation(candidate))
            {
                hintCard = candidate; hintFoundationSuit = candidate.Suit;
                status.Text = F($"Indice : place {CardLabel(candidate)} dans sa fondation.", $"Hint: move {CardLabel(candidate)} to its foundation.", $"Hint: verplaats {CardLabel(candidate)} naar de basisstapel.");
                Refresh(); return;
            }
            for (var from = 0; from < tableau.Length; from++)
            {
                var pile = tableau[from];
                for (var start = pile.FindIndex(card => !card.FaceDown); start >= 0 && start < pile.Count; start++)
                {
                    var card = pile[start];
                    if (start == pile.Count - 1 && CanGoToFoundation(card))
                    {
                        hintCard = card; hintFoundationSuit = card.Suit;
                        status.Text = F($"Indice : place {CardLabel(card)} dans sa fondation.", $"Hint: move {CardLabel(card)} to its foundation.", $"Hint: verplaats {CardLabel(card)} naar de basisstapel.");
                        Refresh(); return;
                    }
                    for (var to = 0; to < tableau.Length; to++)
                        if (to != from && CanPlaceOn(card, tableau[to]))
                        {
                            hintCard = card; hintTargetColumn = to;
                            status.Text = F($"Indice : déplace {CardLabel(card)} vers la colonne {to + 1}.", $"Hint: move {CardLabel(card)} to column {to + 1}.", $"Hint: verplaats {CardLabel(card)} naar kolom {to + 1}.");
                            Refresh(); return;
                        }
                }
            }
            if (candidate is not null)
                for (var to = 0; to < tableau.Length; to++)
                    if (CanPlaceOn(candidate, tableau[to]))
                    {
                        hintCard = candidate; hintTargetColumn = to;
                        status.Text = F($"Indice : déplace {CardLabel(candidate)} vers la colonne {to + 1}.", $"Hint: move {CardLabel(candidate)} to column {to + 1}.", $"Hint: verplaats {CardLabel(candidate)} naar kolom {to + 1}.");
                        Refresh(); return;
                    }
            status.Text = T("Aucun coup évident. Pioche ou retourne une carte cachée."); Refresh();
        }

        bool CanGoToFoundation(SolitaireCard card)
        {
            var pile = foundations[card.Suit];
            return pile.Count == 0 ? card.Rank == 1 : card.Rank == pile[^1].Rank + 1;
        }

        void SelectWaste()
        {
            if (waste.Count == 0) return;
            source = source == SolitaireSource.Waste ? SolitaireSource.None : SolitaireSource.Waste;
            sourceColumn = -1; sourceIndex = waste.Count - 1;
            hintCard = null; hintTargetColumn = -1; hintFoundationSuit = null;
            status.Text = T("Choisis une carte, puis sa destination."); Refresh();
        }

        void DrawStock()
        {
            source = SolitaireSource.None; hintCard = null; hintTargetColumn = -1; hintFoundationSuit = null;
            if (stock.Count > 0)
            {
                SaveUndo(); waste.Add(stock[^1]); stock.RemoveAt(stock.Count - 1); moves++;
                justMovedCard = waste[^1]; status.Text = T("Choisis une carte, puis sa destination.");
            }
            else if (waste.Count > 0)
            {
                SaveUndo(); stock.AddRange(waste.AsEnumerable().Reverse()); waste.Clear(); moves++;
                status.Text = T("Pioche vide. Touche-la pour reprendre les cartes.");
            }
            else status.Text = T("Pioche vide. Touche-la pour reprendre les cartes.");
            Refresh();
        }

        void HandleTableauTap(int column, int cardIndex)
        {
            var pile = tableau[column];
            if (source != SolitaireSource.None)
            {
                if (source == SolitaireSource.Tableau && sourceColumn == column && sourceIndex == cardIndex)
                {
                    source = SolitaireSource.None; hintCard = null; Refresh(); return;
                }
                if (TryMoveToTableau(column)) return;
                status.Text = T("Déplacement impossible.");
                ShowGameFeedback(false, F("Essaie une autre colonne.", "Try another column.", "Probeer een andere kolom."), true);
                return;
            }

            if (cardIndex < 0) return;
            var tapped = pile[cardIndex];
            if (tapped.FaceDown)
            {
                if (cardIndex == pile.Count - 1)
                {
                    SaveUndo(); tapped.FaceDown = false; moves++; score += 5; justMovedCard = tapped;
                    status.Text = T("Choisis une carte, puis sa destination.");
                    Refresh();
                }
                return;
            }
            source = SolitaireSource.Tableau; sourceColumn = column; sourceIndex = cardIndex;
            hintCard = null; hintTargetColumn = -1; hintFoundationSuit = null;
            status.Text = T("Choisis une carte, puis sa destination.");
            Refresh();
        }

        bool TryMoveToTableau(int targetColumn)
        {
            List<SolitaireCard> moving;
            if (source == SolitaireSource.Waste)
                moving = waste.Count == 0 ? new List<SolitaireCard>() : new List<SolitaireCard> { waste[^1] };
            else if (source == SolitaireSource.Tableau && sourceColumn >= 0 && sourceIndex >= 0)
                moving = tableau[sourceColumn].Skip(sourceIndex).ToList();
            else return false;

            if (moving.Count == 0) return false;
            var destination = tableau[targetColumn];
            var first = moving[0];
            if (!CanPlaceOn(first, destination)) return false;

            SaveUndo();
            if (source == SolitaireSource.Waste) waste.RemoveAt(waste.Count - 1);
            else
            {
                tableau[sourceColumn].RemoveRange(sourceIndex, moving.Count);
                if (tableau[sourceColumn].Count > 0 && tableau[sourceColumn][^1].FaceDown)
                    tableau[sourceColumn][^1].FaceDown = false;
            }
            destination.AddRange(moving); moves++; score += 5; justMovedCard = moving[0];
            source = SolitaireSource.None; hintCard = null; hintTargetColumn = -1; hintFoundationSuit = null;
            status.Text = T("Choisis une carte, puis sa destination.");
            Refresh(); return true;
        }

        void MoveToFoundation(char suit)
        {
            if (source == SolitaireSource.None) return;
            List<SolitaireCard> sourcePile;
            if (source == SolitaireSource.Waste) sourcePile = waste;
            else if (source == SolitaireSource.Tableau && sourceIndex == tableau[sourceColumn].Count - 1)
                sourcePile = tableau[sourceColumn];
            else { status.Text = T("Déplacement impossible."); ShowGameFeedback(false, T("Déplacement impossible."), true); return; }

            if (sourcePile.Count == 0) return;
            var card = sourcePile[^1];
            var foundation = foundations[suit];
            var canPlace = card.Suit == suit && (foundation.Count == 0 ? card.Rank == 1 : card.Rank == foundation[^1].Rank + 1);
            if (!canPlace) { status.Text = T("Déplacement impossible."); ShowGameFeedback(false, T("Déplacement impossible."), true); return; }

            SaveUndo();
            sourcePile.RemoveAt(sourcePile.Count - 1);
            foundation.Add(card); moves++; score += 10; justMovedCard = card;
            if (source == SolitaireSource.Tableau && sourcePile.Count > 0 && sourcePile[^1].FaceDown)
                sourcePile[^1].FaceDown = false;
            source = SolitaireSource.None;
            if (foundations.Values.Sum(pile => pile.Count) == 52)
            {
                score += 100;
                status.Text = T("Partie terminée ! Tu as gagné 🎉"); solitaireActive = false;
                ShowGameFeedback(true, F("Solitaire terminé !", "Solitaire complete!", "Patience voltooid!"));
            }
            else status.Text = T("Choisis une carte, puis sa destination.");
            Refresh();
        }

        void Refresh()
        {
            stockSlot.Content = stock.Count > 0 ? CardBack(stock.Count) : EmptyStock();
            wasteSlot.Content = waste.Count == 0 ? new Label { Text = "·", FontSize = 25, TextColor = Color.FromArgb("#92C2AB"), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center }
                : CardFace(waste[^1], source == SolitaireSource.Waste, ReferenceEquals(hintCard, waste[^1]));
            wasteSlot.Stroke = source == SolitaireSource.Waste || ReferenceEquals(hintCard, waste.LastOrDefault()) ? Color.FromArgb("#FFE16A") : Color.FromArgb("#91B29D");
            for (var i = 0; i < suits.Length; i++)
            {
                var pile = foundations[suits[i]];
                foundationSlots[i].Content = pile.Count == 0
                    ? new Label { Text = suits[i].ToString(), FontSize = 25, TextColor = IsRedSuit(suits[i]) ? Color.FromArgb("#73BCA0") : Color.FromArgb("#92C2AB"), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center }
                    : CardFace(pile[^1], false, false);
                foundationSlots[i].Stroke = hintFoundationSuit == suits[i] ? Color.FromArgb("#FFE16A") : Color.FromArgb("#91B29D");
            }
            scoreLabel.Text = score.ToString(); movesLabel.Text = moves.ToString();
            tableauGrid.Children.Clear();
            for (var column = 0; column < tableau.Length; column++)
            {
                var columnIndex = column;
                // Keep the complete card bounds visible, including the lower corners and shadow.
                var pileView = new AbsoluteLayout
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Start,
                    IsClippedToBounds = false
                };
                if (tableau[column].Count == 0)
                {
                    var empty = Slot(null, null, () => HandleTableauTap(columnIndex, -1), hintTargetColumn == columnIndex);
                    AbsoluteLayout.SetLayoutBounds(empty, new Rect(0, 0, 1, 66));
                    AbsoluteLayout.SetLayoutFlags(empty, AbsoluteLayoutFlags.WidthProportional);
                    pileView.Children.Add(empty); pileView.HeightRequest = 66;
                }
                var y = 0d;
                var pileBottom = 0d;
                for (var i = 0; i < tableau[column].Count; i++)
                {
                    var cardIndex = i;
                    var card = tableau[column][i];
                    var selected = source == SolitaireSource.Tableau && sourceColumn == column && cardIndex >= sourceIndex;
                    var faceDown = card.FaceDown;
                    var cardView = faceDown ? CardBack(null) : CardFace(card, selected, ReferenceEquals(hintCard, card));
                    var cardHeight = faceDown ? 38 : 66;
                    AbsoluteLayout.SetLayoutBounds(cardView, new Rect(0, y, 1, cardHeight));
                    AbsoluteLayout.SetLayoutFlags(cardView, AbsoluteLayoutFlags.WidthProportional);
                    var capturedIndex = cardIndex;
                    var cardTap = new TapGestureRecognizer(); cardTap.Tapped += (_, _) => HandleTableauTap(columnIndex, capturedIndex); cardView.GestureRecognizers.Add(cardTap);
                    if (!faceDown && cardIndex == tableau[column].Count - 1 && hintTargetColumn == columnIndex) cardView.Stroke = Color.FromArgb("#FFE16A");
                    if (ReferenceEquals(card, justMovedCard))
                    {
                        cardView.Opacity = 0.5; cardView.Scale = 0.9;
                        _ = cardView.FadeToAsync(1, 190); _ = cardView.ScaleToAsync(1, 210, Easing.SpringOut);
                    }
                    pileView.Children.Add(cardView);
                    pileBottom = y + cardHeight;
                    // Keep the classic overlapping tableau while leaving the face-up card readable.
                    y += faceDown ? 24 : 56;
                }
                if (tableau[column].Count > 0) pileView.HeightRequest = pileBottom + 8;
                tableauGrid.Add(pileView, column, 0);
            }
            if (justMovedCard is not null) justMovedCard = null;
        }

        Border CardFace(SolitaireCard card, bool selected, bool hinted)
        {
            var red = IsRedSuit(card.Suit);
            var ink = red ? Color.FromArgb("#D71931") : Color.FromArgb("#172126");
            Label Corner(bool inverted) => new()
            {
                Text = $"{CardRank(card)}\n{card.Suit}",
                FontSize = 9,
                FontAttributes = FontAttributes.Bold,
                TextColor = ink,
                LineHeight = 0.85,
                LineBreakMode = LineBreakMode.NoWrap,
                HorizontalTextAlignment = inverted ? TextAlignment.End : TextAlignment.Start,
                VerticalTextAlignment = inverted ? TextAlignment.End : TextAlignment.Start,
                Rotation = inverted ? 180 : 0,
                Margin = 0
            };

            var face = new Grid
            {
                Padding = new Thickness(3, 2),
                RowDefinitions =
                {
                    new RowDefinition(new GridLength(13)),
                    new RowDefinition(GridLength.Star),
                    new RowDefinition(new GridLength(13))
                }
            };
            face.Add(Corner(false), 0, 0);
            face.Add(Corner(true), 0, 2);

            var pips = new Grid { ColumnSpacing = 0, RowSpacing = 0 };
            pips.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            pips.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            for (var row = 0; row < 5; row++)
                pips.RowDefinitions.Add(new RowDefinition(GridLength.Star));

            if (card.Rank == 1)
            {
                var ace = new Label
                {
                    Text = card.Suit.ToString(), FontSize = 22, FontAttributes = FontAttributes.Bold,
                    TextColor = ink, HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };
                pips.Add(ace, 0, 0);
                Grid.SetColumnSpan(ace, 2);
                Grid.SetRowSpan(ace, 5);
            }
            else if (card.Rank >= 2 && card.Rank <= 10)
            {
                var positions = card.Rank switch
                {
                    2 => new (int Row, int Column)[] { (0, 0), (4, 1) },
                    3 => new (int, int)[] { (0, 0), (2, -1), (4, 1) },
                    4 => new (int, int)[] { (0, 0), (0, 1), (4, 0), (4, 1) },
                    5 => new (int, int)[] { (0, 0), (0, 1), (2, -1), (4, 0), (4, 1) },
                    6 => new (int, int)[] { (0, 0), (0, 1), (2, 0), (2, 1), (4, 0), (4, 1) },
                    7 => new (int, int)[] { (0, 0), (0, 1), (1, -1), (2, 0), (2, 1), (4, 0), (4, 1) },
                    8 => new (int, int)[] { (0, 0), (0, 1), (1, 0), (1, 1), (3, 0), (3, 1), (4, 0), (4, 1) },
                    9 => new (int, int)[] { (0, 0), (0, 1), (1, 0), (1, 1), (2, -1), (3, 0), (3, 1), (4, 0), (4, 1) },
                    _ => new (int, int)[] { (0, 0), (0, 1), (1, 0), (1, 1), (2, 0), (2, 1), (3, 0), (3, 1), (4, 0), (4, 1) }
                };
                foreach (var (row, column) in positions)
                {
                    var pip = new Label
                    {
                        Text = card.Suit.ToString(), FontSize = 7, FontAttributes = FontAttributes.Bold,
                        TextColor = ink, HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center, LineBreakMode = LineBreakMode.NoWrap,
                        Rotation = row >= 3 && column >= 0 ? 180 : 0, Margin = 0
                    };
                    var actualColumn = column < 0 ? 0 : column;
                    pips.Add(pip, actualColumn, row);
                    if (column < 0) Grid.SetColumnSpan(pip, 2);
                }
            }
            else
            {
                var court = new VerticalStackLayout
                {
                    Spacing = -2,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label { Text = CardRank(card), FontSize = 19, FontAttributes = FontAttributes.Bold, TextColor = ink, HorizontalTextAlignment = TextAlignment.Center },
                        new Label { Text = card.Suit.ToString(), FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = ink, HorizontalTextAlignment = TextAlignment.Center }
                    }
                };
                pips.Add(court, 0, 0);
                Grid.SetColumnSpan(court, 2);
                Grid.SetRowSpan(court, 5);
            }
            face.Add(pips, 0, 1);
            var border = new Border { HeightRequest = 66, Padding = 0, BackgroundColor = Colors.White,
                Stroke = hinted || selected ? Color.FromArgb("#FFE16A") : Color.FromArgb("#DCE4E0"), StrokeThickness = hinted || selected ? 2.5 : 1,
                StrokeShape = new RoundRectangle { CornerRadius = 7 }, Content = face,
                Shadow = new Shadow { Brush = Color.FromArgb("#50001810"), Offset = new Point(0, 3), Radius = 4, Opacity = 0.42f } };
            if (selected) border.TranslationY = -5;
            return border;
        }

        Border CardBack(int? count)
        {
            var inside = new Border { Margin = 3, Padding = 0, Stroke = Color.FromArgb("#F7DDE0"), StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 5 },
                Content = new Label { Text = "✥", FontSize = 23, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#FCE8E9"), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center } };
            var grid = new Grid { BackgroundColor = Color.FromArgb("#C9233C"), Children = { inside } };
            if (count.HasValue) grid.Add(new Label { Text = count.Value.ToString(), FontSize = 10, FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White, BackgroundColor = Color.FromArgb("#A71930"), HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center, WidthRequest = 18, HeightRequest = 17, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.End }, 0, 0);
            return new Border { WidthRequest = 46, HeightRequest = count.HasValue ? 66 : 38, Padding = 2,
                BackgroundColor = Color.FromArgb("#B51F37"), Stroke = Colors.White, StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 7 }, Content = grid,
                Shadow = new Shadow { Brush = Color.FromArgb("#50001810"), Offset = new Point(0, 3), Radius = 4, Opacity = 0.4f } };
        }

        View EmptyStock() => new Label { Text = "↻", FontSize = 23, TextColor = Colors.White,
            HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

        Refresh();
        solitaireActive = true;
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!solitaireActive || !body.Children.Contains(tableauGrid)) return false;
            elapsedSeconds++;
            timeLabel.Text = $"{elapsedSeconds / 60:00}:{elapsedSeconds % 60:00}";
            return true;
        });
    }

    static bool IsRedSuit(char suit) => suit is '♥' or '♦';

    static string CardRank(SolitaireCard card) => card.Rank switch
    {
        1 => "A", 11 => "J", 12 => "Q", 13 => "K", _ => card.Rank.ToString()
    };

    static string CardLabel(SolitaireCard card) => CardRank(card) + card.Suit;

    static Button MakeCardButton(string label, Color background, Color textColor) => new()
    {
        Text = label, FontSize = 13, FontAttributes = FontAttributes.Bold,
        Padding = 0, Margin = 0, CornerRadius = 5, HeightRequest = 46,
        BackgroundColor = background, TextColor = textColor
    };

    void PlayDice()
    {
        StartPage("Lance le dé", "Lance le dé et essaie d’obtenir un six !");
        var die = new WebView
        {
            Source = new HtmlWebViewSource { Html = Dice3D.Page },
            HeightRequest = 280,
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center
        };
        var dieReady = false;
        die.Navigated += (_, _) => dieReady = true;
        var result = Text("À toi de jouer !", 20, true, Muted);
        var rollButton = MakeButton("Lancer le dé", Green, () => { }, 58);
        var rolling = false;
        rollButton.Clicked += async (_, _) =>
        {
            if (rolling || !dieReady) return;
            rolling = true;
            rollButton.IsEnabled = false;
            result.Text = T("Ça tourne…");
            var number = random.Next(1, 7);
            await die.EvaluateJavaScriptAsync($"rollDice({number})");
            await Task.Delay(1750);
            result.Text = F($"Résultat : {number}", $"Result: {number}", $"Resultaat: {number}");
            if (number == 6)
                AddPoints(3);
            rollButton.IsEnabled = true;
            rolling = false;
        };
        body.Children.Add(Panel(new VerticalStackLayout
        {
            Spacing = 12,
            Padding = new Thickness(10, 18),
            Children = { die, result }
        }, Color.FromArgb("#FFF0EE")));
        body.Children.Add(rollButton);
    }

    void PlayReflex()
    {
        StartPage("Réflexe", "Attends le vert… puis appuie vite !");
        armed = false; running = true;
        var recordMs = Preferences.Default.Get("reflex-record-ms", 0);
        string RecordText() => recordMs == 0
            ? F("Record à battre : aucun", "Record to beat: none", "Te verbreken record: geen")
            : F($"Record à battre : {recordMs} ms", $"Record to beat: {recordMs} ms", $"Te verbreken record: {recordMs} ms");
        var recordLabel = Text(RecordText(), 15, true, Purple);
        var status = Text("Patiente un instant…", 18, true, Muted);
        Button circle = null!;
        circle = MakeButton("🟠", Color.FromArgb("#ED7C55"), () =>
        {
            if (!running) return;
            if (!armed)
            {
                running = false; status.Text = T("Trop tôt ! Essaie encore."); circle.Text = "🙈";
                ShowGameFeedback(false, F("Tu as appuyé trop tôt.", "You tapped too early.", "Je tikte te vroeg."));
                body.Children.Add(MakeButton("Rejouer", Purple, PlayReflex)); return;
            }
            running = false;
            var milliseconds = (DateTime.UtcNow - startAt).TotalMilliseconds;
            var reward = Math.Max(1, 10 - (int)(milliseconds / 100)); AddPoints(reward);
            var newRecord = recordMs == 0 || milliseconds < recordMs;
            if (newRecord)
            {
                recordMs = (int)Math.Round(milliseconds);
                Preferences.Default.Set("reflex-record-ms", recordMs);
                recordLabel.Text = RecordText();
            }
            status.Text = F($"{milliseconds:0} ms — +{reward} points !", $"{milliseconds:0} ms — +{reward} points!", $"{milliseconds:0} ms — +{reward} punten!");
            var feedback = newRecord
                ? F($"Nouveau record : {recordMs} ms !", $"New record: {recordMs} ms!", $"Nieuw record: {recordMs} ms!")
                : F($"Temps : {milliseconds:0} ms. Record à battre : {recordMs} ms.", $"Time: {milliseconds:0} ms. Record to beat: {recordMs} ms.", $"Tijd: {milliseconds:0} ms. Te verbreken record: {recordMs} ms.");
            ShowGameFeedback(true, feedback);
            body.Children.Add(MakeButton("Encore", Purple, PlayReflex));
        }, 230);
        circle.FontSize = 70;
        circle.Shadow = new Shadow { Brush = Color.FromArgb("#50ED7C55"), Offset = new Point(0, 8), Radius = 16, Opacity = 0.38f };
        body.Children.Add(Panel(circle, Color.FromArgb("#FFF0EB")));
        body.Children.Add(recordLabel);
        body.Children.Add(status);
        Task.Delay(random.Next(1500, 4500)).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
        {
            if (!running) return;
            armed = true; startAt = DateTime.UtcNow; circle.Text = "🟢";
            circle.BackgroundColor = Green; status.Text = T("MAINTENANT !");
        }));
    }

    void PlayMinesweeper()
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
        reset.Clicked += (_, _) => PlayMinesweeper();
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

    void PlayCoin()
    {
        StartPage("Pile ou face", "Pas besoin de choisir : lance la pièce !");
        var coin = Text("🪙", 100, true); var result = Text("Pile… ou face ?", 22, true);
        coin.Shadow = new Shadow { Brush = Color.FromArgb("#55D89A2B"), Offset = new Point(0, 8), Radius = 14, Opacity = 0.38f };
        var flipping = false;
        var flipButton = MakeButton("Lancer la pièce", Color.FromArgb("#D89A2B"), () => { });
        flipButton.Clicked += async (_, _) =>
        {
            if (flipping) return;
            flipping = true; flipButton.IsEnabled = false;
            result.Text = T("La pièce tourne…");
            for (var i = 0; i < 6; i++)
            {
                coin.Text = i % 2 == 0 ? "🟡" : "⚪";
                await coin.RotateYToAsync(coin.RotationY + 180, 110, Easing.CubicInOut);
            }
            var pile = random.Next(2) == 0; coin.Text = pile ? "🟡" : "⚪";
            result.Text = T(pile ? "PILE !" : "FACE !");
            await coin.ScaleToAsync(1.12, 100); await coin.ScaleToAsync(1, 180, Easing.SpringOut);
            flipButton.IsEnabled = true; flipping = false;
        };
        body.Children.Add(Panel(new VerticalStackLayout { Spacing = 12, Padding = new Thickness(12, 20), Children = { coin, result } }, Color.FromArgb("#FFF7E4")));
        body.Children.Add(flipButton);
    }

    void AddPoints(int amount)
    {
        points += amount;
        Preferences.Default.Set("points", points);
    }
}
