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
        ["Attrape les étoiles"] = ["Catch the stars", "Vang de sterren"], ["Tape vite avant la fin"] = ["Tap quickly before time runs out", "Tik snel voordat de tijd om is"],
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
    readonly VerticalStackLayout body = new() { Spacing = 16, Padding = new Thickness(20, 18, 20, 28) };
    readonly Random random = new();
    int points = Preferences.Default.Get("points", 0);
    int taps;
    int seconds;
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
            ("🎯", "Attrape les étoiles", "Tape vite avant la fin", "#FFF7D9", PlayStars),
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
            var button = new Button { FontSize = 28, FontAttributes = FontAttributes.Bold, CornerRadius = 17 };
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

    void PlayStars()
    {
        StartPage("Attrape les étoiles", "Tape la case avec l’étoile avant la fin du chrono !");
        taps = 0; seconds = 20; running = true;
        var timer = Text(F("20 secondes", "20 seconds", "20 seconden"), 17, true, Muted);
        var score = Text(F("0 étoile", "0 stars", "0 sterren"), 20, true);
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
                taps++; score.Text = F($"{taps} étoile{(taps == 1 ? "" : "s")} !", $"{taps} star{(taps == 1 ? "" : "s")}!", $"{taps} ster{(taps == 1 ? "" : "ren")}!");
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
            seconds--; timer.Text = F($"{seconds} seconde{(seconds == 1 ? "" : "s")}", $"{seconds} second{(seconds == 1 ? "" : "s")}", $"{seconds} seconde{(seconds == 1 ? "" : "n")}");
            if (seconds > 0) return true;
            running = false; AddPoints(taps); score.Text = F($"Terminé ! {taps} étoiles attrapées 🎉", $"Time's up! You caught {taps} stars 🎉", $"Tijd is om! Je ving {taps} sterren 🎉");
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
            result.Text = T("Ça tourne…");
            for (var i = 0; i < 7; i++)
            {
                face.Text = new[] { "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" }[random.Next(6)];
                await Task.Delay(80);
            }
            var number = random.Next(1, 7);
            face.Text = new[] { "", "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" }[number];
            result.Text = number == 6 ? T("Un six ! +3 points 🎉") : F($"Tu as obtenu {number}. Encore ?", $"You rolled {number}. Again?", $"Je gooide {number}. Nog een keer?");
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
            Padding = new Thickness(8, 4), Margin = new Thickness(4)
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
