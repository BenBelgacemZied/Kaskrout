using Microsoft.Maui.Controls.Shapes;

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
        ["Lance le dé"] = ["Roll the dice", "Gooi de dobbelsteen"], ["Un lancer porte-bonheur ?"] = ["A lucky roll?", "Een gelukkige worp?"],
        ["Réflexe"] = ["Reflex", "Reflex"], ["Attends le vert et appuie"] = ["Wait for green, then tap", "Wacht op groen en tik"],
        ["XP Minesweeper Classic"] = ["XP Minesweeper Classic", "XP Minesweeper Classic"],
        ["Trouve les cases sûres"] = ["Find the safe squares", "Vind de veilige vakjes"],
        ["Découvre toutes les cases sans mine. Active le mode drapeau pour signaler un danger."] = ["Reveal every square without a mine. Turn on flag mode to mark a danger.", "Onthul alle vakjes zonder mijn. Zet de vlagmodus aan om gevaar aan te geven."],
        ["Drapeaux : désactivés"] = ["Flags: off", "Vlaggen: uit"], ["Drapeaux : activés"] = ["Flags: on", "Vlaggen: aan"],
        ["Première case sûre. À toi de jouer !"] = ["First square is safe. Your turn!", "Eerste vakje is veilig. Jij bent aan de beurt!"],
        ["Mines révélées ! Recommence pour tenter ta chance."] = ["Mine revealed! Start a new game and try again.", "Mijn gevonden! Start een nieuw spel en probeer opnieuw."],
        ["Grille nettoyée ! +10 points 🎉"] = ["Board cleared! +10 points 🎉", "Bord leeggemaakt! +10 punten 🎉"],
        ["Pile ou face"] = ["Heads or tails", "Kop of munt"], ["La pièce choisit pour toi"] = ["Let the coin choose for you", "Laat de munt voor je kiezen"],
        ["Machine surprise"] = ["Surprise machine", "Verrassingsmachine"], ["Quel emoji va sortir ?"] = ["Which emoji will appear?", "Welke emoji verschijnt er?"],
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
        ["Appuie et découvre ton emoji porte-bonheur !"] = ["Tap and discover your lucky emoji!", "Tik en ontdek je geluks-emoji!"], ["Qui va apparaître ?"] = ["Who will appear?", "Wie verschijnt er?"], ["Surprise !"] = ["Surprise!", "Verrassing!"], ["Roulement…"] = ["Rolling…", "Spannend…"], ["Encore ?"] = ["Again?", "Nog een keer?"],
        ["seconde"] = ["second", "seconde"], ["secondes"] = ["seconds", "seconden"], ["étoile"] = ["star", "ster"], ["étoiles"] = ["stars", "sterren"],
        ["Terminé !"] = ["Time's up!", "Tijd is om!"], ["Tu as trouvé"] = ["You found", "Je vond"], ["objets sur"] = ["objects out of", "voorwerpen van"], ["C’était"] = ["It was", "Het was"], ["Bravo ! Puzzle terminé en"] = ["Great! Puzzle completed in", "Goed gedaan! Puzzel opgelost in"], ["coups"] = ["moves", "zetten"], ["Tu as obtenu"] = ["You rolled", "Je gooide"], ["Un six ! +3 points 🎉"] = ["A six! +3 points 🎉", "Zes! +3 punten 🎉"],
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
    readonly VerticalStackLayout body = new() { Spacing = 16, Padding = new Thickness(20, 18, 20, 28) };
    readonly Random random = new();
    int points = Preferences.Default.Get("points", 0);
    bool running;
    bool armed;
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
            ("💣", "XP Minesweeper Classic", "Trouve les cases sûres", "#E8EEF5", PlayMinesweeper),
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
            var button = new Button { FontSize = 28, FontAttributes = FontAttributes.Bold, CornerRadius = 17,
                Shadow = new Shadow { Brush = Color.FromArgb("#90715CE8"), Offset = new Point(0, 5), Radius = 7, Opacity = 0.32f } };
            button.Clicked += (_, _) =>
            {
                if (index == empty || !IsNeighbor(index, empty)) return;
                (tiles[index], tiles[empty]) = (tiles[empty], tiles[index]);
                empty = index; moves++; status.Text = F($"Coups : {moves}", $"Moves: {moves}", $"Zetten: {moves}");
                RefreshTiles();
                if (tiles.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 }))
                {
                    status.Text = F($"Bravo ! Puzzle terminé en {moves} coups 🎉", $"Great! Puzzle completed in {moves} moves 🎉", $"Goed gedaan! Puzzel opgelost in {moves} zetten 🎉");
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
                    Children = { Text("Bien joué !", 26, true, Green), Text(F($"Tu as trouvé {score} objets sur {rounds.Length}.", $"You found {score} objects out of {rounds.Length}.", $"Je vond {score} voorwerpen van de {rounds.Length}."), 18) }
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
                question.Text = T("Lequel a disparu ?");
                var decoys = new[] { "🍋", "🛴", "🐶", "🎁", "☀️", "🍪", "🚕", "🐻", "🎾", "🍇" }
                    .Where(x => !items.Contains(x)).OrderBy(_ => random.Next()).Take(3).ToList();
                foreach (var option in decoys.Append(missing).OrderBy(_ => random.Next()))
                {
                    var answer = MakeButton(option, Color.FromArgb("#F2F0FB"), () =>
                    {
                        if (optionsArea.Children.All(x => !x.IsEnabled)) return;
                        if (option == missing) { score++; question.Text = T("Exactement ! ✨"); }
                        else question.Text = F($"C’était {missing} !", $"It was {missing}!", $"Het was {missing}!");
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
                BackgroundColor = Color.FromArgb("#EEEAFE"), TextColor = Purple, CornerRadius = 17,
                Shadow = new Shadow { Brush = Color.FromArgb("#90715CE8"), Offset = new Point(0, 5), Radius = 7, Opacity = 0.3f } };
            card.Clicked += async (_, _) =>
            {
                if (busy || matched.Contains(index) || open.Contains(index)) return;
                await card.RotateYToAsync(90, 90);
                card.Text = symbols[index]; open.Add(index);
                await card.RotateYToAsync(0, 100);
                if (open.Count < 2) return;
                busy = true;
                await Task.Delay(650);
                var first = open[0]; var second = open[1]; open.Clear();
                if (symbols[first] == symbols[second])
                {
                    matched.Add(first); matched.Add(second); matches++; AddPoints(2);
                    cards[first].BackgroundColor = Color.FromArgb("#DFF4EC");
                    cards[second].BackgroundColor = Color.FromArgb("#DFF4EC");
                    status.Text = matches == 6 ? T("Toutes les paires trouvées ! 🎉") : F($"Paires trouvées : {matches} / 6", $"Pairs found: {matches} / 6", $"Paren gevonden: {matches} / 6");
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

    void PlaySolitaire()
    {
        StartPage("Solitaire", "Touche la pioche pour tirer. Range les cartes par couleur, de l’as au roi.");
        var suits = new[] { '♠', '♥', '♦', '♣' };
        var deck = (from suit in suits from rank in Enumerable.Range(1, 13) select new SolitaireCard(rank, suit))
            .OrderBy(_ => random.Next()).ToList();
        var stock = new List<SolitaireCard>();
        var waste = new List<SolitaireCard>();
        var tableau = Enumerable.Range(0, 7).Select(_ => new List<SolitaireCard>()).ToArray();
        var foundations = suits.ToDictionary(suit => suit, _ => new List<SolitaireCard>());
        var moves = 0;
        var source = SolitaireSource.None;
        var sourceColumn = -1;
        var sourceIndex = -1;
        var status = Text("Choisis une carte, puis sa destination.", 14, true, Muted);
        var moveCount = Text(F("Coups : 0", "Moves: 0", "Zetten: 0"), 15, true, Purple);

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

        var topRow = new Grid { ColumnSpacing = 5, HeightRequest = 62 };
        for (var i = 0; i < 6; i++)
            topRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        var stockButton = new Button { FontSize = 17, Padding = 0, CornerRadius = 7, HeightRequest = 58,
            BackgroundColor = Color.FromArgb("#715CE8"), TextColor = Colors.White };
        var wasteButton = new Button { FontSize = 15, Padding = 0, CornerRadius = 7, HeightRequest = 58,
            BackgroundColor = Colors.White, TextColor = Ink };
        var foundationButtons = new Button[4];
        stockButton.Clicked += (_, _) =>
        {
            source = SolitaireSource.None;
            if (stock.Count > 0)
            {
                waste.Add(stock[^1]); stock.RemoveAt(stock.Count - 1); moves++;
                status.Text = T("Choisis une carte, puis sa destination.");
            }
            else if (waste.Count > 0)
            {
                stock.AddRange(waste.AsEnumerable().Reverse()); waste.Clear(); moves++;
                status.Text = T("Pioche vide. Touche-la pour reprendre les cartes.");
            }
            else status.Text = T("Pioche vide. Touche-la pour reprendre les cartes.");
            Refresh();
        };
        wasteButton.Clicked += (_, _) =>
        {
            if (waste.Count == 0) return;
            if (source == SolitaireSource.Waste) source = SolitaireSource.None;
            else { source = SolitaireSource.Waste; sourceColumn = -1; sourceIndex = waste.Count - 1; }
            status.Text = T("Choisis une carte, puis sa destination.");
            Refresh();
        };
        topRow.Add(stockButton, 0, 0); topRow.Add(wasteButton, 1, 0);
        for (var i = 0; i < suits.Length; i++)
        {
            var suit = suits[i];
            var button = new Button { FontSize = 17, Padding = 0, CornerRadius = 7, HeightRequest = 58,
                BackgroundColor = Colors.White, TextColor = IsRedSuit(suit) ? Color.FromArgb("#D23A45") : Ink };
            button.Clicked += (_, _) => MoveToFoundation(suit);
            foundationButtons[i] = button;
            topRow.Add(button, i + 2, 0);
        }

        var tableauGrid = new Grid { ColumnSpacing = 3, RowSpacing = 0, HorizontalOptions = LayoutOptions.Fill };
        for (var i = 0; i < 7; i++)
            tableauGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        body.Children.Add(Panel(new VerticalStackLayout { Spacing = 8, Children = { topRow, moveCount } }, Color.FromArgb("#EAF3EA")));
        body.Children.Add(tableauGrid);
        body.Children.Add(status);

        void HandleTableauTap(int column, int cardIndex)
        {
            var pile = tableau[column];
            if (source != SolitaireSource.None)
            {
                if (source == SolitaireSource.Tableau && sourceColumn == column && sourceIndex == cardIndex)
                {
                    source = SolitaireSource.None; Refresh(); return;
                }
                if (TryMoveToTableau(column)) return;
                status.Text = T("Déplacement impossible.");
                return;
            }

            if (cardIndex < 0) return;
            var tapped = pile[cardIndex];
            if (tapped.FaceDown)
            {
                if (cardIndex == pile.Count - 1)
                {
                    tapped.FaceDown = false; moves++;
                    status.Text = T("Choisis une carte, puis sa destination.");
                    Refresh();
                }
                return;
            }
            source = SolitaireSource.Tableau; sourceColumn = column; sourceIndex = cardIndex;
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
            var canPlace = destination.Count == 0
                ? first.Rank == 13
                : !destination[^1].FaceDown && destination[^1].Rank == first.Rank + 1
                    && IsRedSuit(destination[^1].Suit) != IsRedSuit(first.Suit);
            if (!canPlace) return false;

            if (source == SolitaireSource.Waste) waste.RemoveAt(waste.Count - 1);
            else
            {
                tableau[sourceColumn].RemoveRange(sourceIndex, moving.Count);
                if (tableau[sourceColumn].Count > 0 && tableau[sourceColumn][^1].FaceDown)
                    tableau[sourceColumn][^1].FaceDown = false;
            }
            destination.AddRange(moving); moves++;
            source = SolitaireSource.None; status.Text = T("Choisis une carte, puis sa destination.");
            Refresh(); return true;
        }

        void MoveToFoundation(char suit)
        {
            if (source == SolitaireSource.None) return;
            List<SolitaireCard> sourcePile;
            if (source == SolitaireSource.Waste) sourcePile = waste;
            else if (source == SolitaireSource.Tableau && sourceIndex == tableau[sourceColumn].Count - 1)
                sourcePile = tableau[sourceColumn];
            else { status.Text = T("Déplacement impossible."); return; }

            if (sourcePile.Count == 0) return;
            var card = sourcePile[^1];
            var foundation = foundations[suit];
            var canPlace = card.Suit == suit && (foundation.Count == 0 ? card.Rank == 1 : card.Rank == foundation[^1].Rank + 1);
            if (!canPlace) { status.Text = T("Déplacement impossible."); return; }

            sourcePile.RemoveAt(sourcePile.Count - 1);
            foundation.Add(card); moves++;
            if (source == SolitaireSource.Tableau && sourcePile.Count > 0 && sourcePile[^1].FaceDown)
                sourcePile[^1].FaceDown = false;
            source = SolitaireSource.None;
            status.Text = foundations.Values.Sum(pile => pile.Count) == 52
                ? T("Partie terminée ! Tu as gagné 🎉")
                : T("Choisis une carte, puis sa destination.");
            Refresh();
        }

        void Refresh()
        {
            stockButton.Text = stock.Count > 0 ? $"▧\n{stock.Count}" : waste.Count > 0 ? "↻" : "·";
            wasteButton.Text = waste.Count == 0 ? "·" : CardLabel(waste[^1]);
            wasteButton.BackgroundColor = source == SolitaireSource.Waste ? Color.FromArgb("#FFE69A") : Colors.White;
            for (var i = 0; i < suits.Length; i++)
            {
                var pile = foundations[suits[i]];
                foundationButtons[i].Text = pile.Count == 0 ? suits[i].ToString() : CardLabel(pile[^1]);
                foundationButtons[i].TextColor = IsRedSuit(suits[i]) ? Color.FromArgb("#D23A45") : Ink;
            }
            moveCount.Text = F($"Coups : {moves}", $"Moves: {moves}", $"Zetten: {moves}");
            tableauGrid.Children.Clear();
            for (var column = 0; column < tableau.Length; column++)
            {
                var columnIndex = column;
                var pileView = new VerticalStackLayout { Spacing = 3, HorizontalOptions = LayoutOptions.Fill };
                if (tableau[column].Count == 0)
                {
                    var empty = MakeCardButton("K", Color.FromArgb("#E8ECE9"), Muted);
                    empty.Clicked += (_, _) => HandleTableauTap(columnIndex, -1);
                    pileView.Children.Add(empty);
                }
                for (var i = 0; i < tableau[column].Count; i++)
                {
                    var cardIndex = i;
                    var card = tableau[column][i];
                    var selected = source == SolitaireSource.Tableau && sourceColumn == column && cardIndex >= sourceIndex;
                    var faceDown = card.FaceDown;
                    var cardButton = MakeCardButton(faceDown ? "▧" : CardLabel(card),
                        faceDown ? Color.FromArgb("#715CE8") : selected ? Color.FromArgb("#FFE69A") : Colors.White,
                        faceDown ? Colors.White : IsRedSuit(card.Suit) ? Color.FromArgb("#D23A45") : Ink);
                    cardButton.HeightRequest = faceDown ? 35 : 46;
                    cardButton.Clicked += (_, _) => HandleTableauTap(columnIndex, cardIndex);
                    pileView.Children.Add(cardButton);
                }
                tableauGrid.Add(pileView, column, 0);
            }
        }

        Refresh();
    }

    static bool IsRedSuit(char suit) => suit is '♥' or '♦';

    static string CardLabel(SolitaireCard card)
    {
        var rank = card.Rank switch { 1 => "A", 11 => "J", 12 => "Q", 13 => "K", _ => card.Rank.ToString() };
        return rank + card.Suit;
    }

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
            result.Text = number == 6 ? T("Un six ! +3 points 🎉") : F($"Tu as obtenu {number}. Encore ?", $"You rolled {number}. Again?", $"Je gooide {number}. Nog een keer?");
            if (number == 6) AddPoints(3);
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
        var status = Text("Patiente un instant…", 18, true, Muted);
        Button circle = null!;
        circle = MakeButton("🟠", Color.FromArgb("#ED7C55"), () =>
        {
            if (!running) return;
            if (!armed)
            {
                running = false; status.Text = T("Trop tôt ! Essaie encore."); circle.Text = "🙈";
                body.Children.Add(MakeButton("Rejouer", Purple, PlayReflex)); return;
            }
            running = false;
            var milliseconds = (DateTime.UtcNow - startAt).TotalMilliseconds;
            var reward = Math.Max(1, 10 - (int)(milliseconds / 100)); AddPoints(reward);
            status.Text = F($"{milliseconds:0} ms — +{reward} points !", $"{milliseconds:0} ms — +{reward} points!", $"{milliseconds:0} ms — +{reward} punten!");
            body.Children.Add(MakeButton("Encore", Purple, PlayReflex));
        }, 230);
        circle.FontSize = 70;
        body.Children.Add(Panel(circle, Color.FromArgb("#FFF0EB")));
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
        StartPage("XP Minesweeper Classic", T("Découvre toutes les cases sans mine. Active le mode drapeau pour signaler un danger."));

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

        Label Counter() => new()
        {
            Text = "000", FontSize = 22, FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#FF3030"), BackgroundColor = Color.FromArgb("#202020"),
            HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center,
            WidthRequest = 76, HeightRequest = 38, Margin = new Thickness(4)
        };
        var mineCounter = Counter();
        var clock = Counter();
        var reset = new Button
        {
            Text = "🙂", FontSize = 26, WidthRequest = 50, HeightRequest = 48,
            Padding = 0, Margin = new Thickness(4), CornerRadius = 4,
            BackgroundColor = Color.FromArgb("#C0C0C0")
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
            BackgroundColor = Color.FromArgb("#C0C0C0"), Padding = 4
        };
        header.Add(mineCounter, 0, 0);
        header.Add(reset, 1, 0);
        header.Add(clock, 2, 0);

        var grid = new Grid { RowSpacing = 2, ColumnSpacing = 2, HeightRequest = 360 };
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
            mineCounter.Text = (mineTotal - flagCount).ToString("000");
            clock.Text = Math.Min(elapsed, 999).ToString("000");
            for (var row = 0; row < size; row++)
                for (var column = 0; column < size; column++)
                {
                    var cell = cells[row, column];
                    cell.IsEnabled = !gameOver;
                    cell.BackgroundColor = opened[row, column]
                        ? Color.FromArgb("#E8E8E8")
                        : Color.FromArgb("#C0C0C0");
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
            reset.Text = won ? "😎" : "😵";
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
            }
            else
                status.Text = T("Mines révélées ! Recommence pour tenter ta chance.");
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
                    Padding = 0, Margin = 0, CornerRadius = 3,
                    BackgroundColor = Color.FromArgb("#C0C0C0"), TextColor = Ink
                };
                cell.Clicked += (_, _) =>
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
                    BackgroundColor = Color.FromArgb("#808080"), StrokeThickness = 0,
                    Padding = 4, Content = grid
                }
            }
        }, Color.FromArgb("#C0C0C0"));
        body.Children.Add(boardPanel);
        body.Children.Add(flagButton);
        body.Children.Add(status);
        RefreshBoard();

        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (gameOver || !body.Children.Contains(boardPanel)) return false;
            if (!generated) return true;
            elapsed++;
            clock.Text = Math.Min(elapsed, 999).ToString("000");
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
        body.Children.Add(Panel(new VerticalStackLayout { Spacing = 12, Padding = new Thickness(12, 20), Children = { coin, result } }, Color.FromArgb("#FFF7E4")));
        body.Children.Add(MakeButton("Lancer la pièce", Color.FromArgb("#E7A735"), async () =>
        {
            result.Text = T("La pièce tourne…");
            for (var i = 0; i < 6; i++) { coin.Text = i % 2 == 0 ? "🟡" : "⚪"; await Task.Delay(100); }
            var pile = random.Next(2) == 0; coin.Text = pile ? "🟡" : "⚪"; result.Text = T(pile ? "PILE !" : "FACE !");
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
            message.Text = T("Roulement…");
            for (var i = 0; i < 8; i++) { display.Text = emojis[random.Next(emojis.Length)]; await Task.Delay(90); }
            display.Text = emojis[random.Next(emojis.Length)]; message.Text = T("Encore ?");
        }));
    }

    void AddPoints(int amount)
    {
        points += amount;
        Preferences.Default.Set("points", points);
    }
}
