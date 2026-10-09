using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using Kaskrout;

namespace Kaskrout;

public class MainPage : ContentPage, IGameUiHost
{
    readonly string language;
    readonly IGameSettingsStore gameSettings = new PreferencesGameSettingsStore();
    readonly IPointsService pointsService;
    readonly GameCatalog gameCatalog = GameCatalog.CreateDefault();
    int points => pointsService.Balance;
    static readonly Dictionary<string, string[]> Translations = new()
    {
        ["Une petite pause ?"] = ["A little break?", "Even pauze?"] ,
        ["Choisis un mini-défi et amuse-toi !"] = ["Pick a mini challenge and have fun!", "Kies een mini-uitdaging en veel plezier!"],
        ["points"] = ["points", "punten"], ["CHOISIS TON JEU"] = ["CHOOSE A GAME", "KIES EEN SPEL"],
        ["Puzzle"] = ["Puzzle", "Puzzel"], ["Remets les tuiles en ordre"] = ["Put the tiles in order", "Zet de tegels op volgorde"],
        ["Objet manquant"] = ["Missing object", "Ontbrekend voorwerp"], ["Observe, puis retrouve-le"] = ["Look, then find what's missing", "Kijk goed en vind wat ontbreekt"],
        ["Paires"] = ["Matching pairs", "Paren"], ["Associe les images identiques"] = ["Match the identical pictures", "Zoek de gelijke plaatjes"],
        ["Chasse aux oiseaux – rétro"] = ["Retro bird hunt", "Retro vogeljacht"], ["Vise les oiseaux et appuie pour tirer."] = ["Aim at the birds and tap to shoot.", "Richt op de vogels en tik om te schieten."],
        ["Solitaire"] = ["Solitaire", "Patience"], ["Jeu de cartes classique"] = ["Classic card game", "Klassiek kaartspel"],
        ["Snake"] = ["Snake", "Snake"], ["Guide le serpent et mange les pommes."] = ["Guide the snake and eat the apples.", "Bestuur de slang en eet de appels."],
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
    readonly VerticalStackLayout body = new() { Spacing = 16, Padding = new Thickness(20, 18, 20, 28) };
    readonly Image pageBackdrop = new() { Aspect = Aspect.AspectFill, Opacity = 0.88, InputTransparent = true };
    readonly BoxView backdropWash = new() { Color = Colors.White, Opacity = 0.34, InputTransparent = true };
    readonly Random random = new();
    readonly IGameSettingsStore gameSettings = new PreferencesGameSettingsStore();
    readonly IPointsService pointsService;
    readonly GameCatalog gameCatalog = GameCatalog.CreateDefault();
    int points => pointsService.Balance;
    Border? activeFeedback;

    public MainPage(string language = "fr")
    {
        this.language = language;
        pointsService = new PreferencesPointsService(gameSettings);
        pointsService = new PreferencesPointsService(gameSettings);
        Title = "Kaskrout";
        BackgroundColor = Paper;
        var page = new Grid();
        page.Children.Add(pageBackdrop);
        page.Children.Add(backdropWash);
        page.Children.Add(new ScrollView { Content = body, BackgroundColor = Colors.Transparent });
        Content = page;
        SetBackdrop("bg_home.jpg");
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
        activeFeedback = null;
        SetBackdrop("bg_home.jpg");
        BackgroundColor = Color.FromArgb("#F3F2FA");
        body.BackgroundColor = Colors.Transparent;
        body.Spacing = 15;
        body.Padding = new Thickness(18, 18, 18, 28);
        body.Children.Clear();

        var games = new (string Icon, string Title, string Detail, string Color, string Category, Action Play)[]
        {
            ("🧩", "Puzzle", "Remets les tuiles en ordre", "#EEEAFE", "Réflexion", () => LaunchGame(GameIds.Puzzle)),
            ("👀", "Objet manquant", "Observe, puis retrouve-le", "#E4F5F1", "Réflexion", () => LaunchGame(GameIds.MissingObject)),
            ("🎴", "Paires", "Associe les images identiques", "#FFF0E4", "Réflexion", () => LaunchGame(GameIds.Pairs)),
            ("🐍", "Snake", "Guide le serpent et mange les pommes.", "#E5F6E8", "Arcade", () => LaunchGame(GameIds.Snake)),
            ("🎲", "Lance le dé", "Un lancer porte-bonheur ?", "#E8F3FF", "Rapide", () => LaunchGame(GameIds.Dice)),
            ("⚡", "Réflexe", "Attends le vert et appuie", "#FFE9EC", "Rapide", () => LaunchGame(GameIds.Reflex)),
            ("💎", "Démineur", "Repère les cases sûres", "#E8EEF5", "Réflexion", () => LaunchGame(GameIds.Minesweeper)),
            ("🪙", "Pile ou face", "La pièce choisit pour toi", "#FFF2D3", "Rapide", () => LaunchGame(GameIds.Coin)),
            ("🕹️", "Chasse aux oiseaux – rétro", "Vise les oiseaux et appuie pour tirer.", "#E7F4FF", "Arcade", () => LaunchGame(GameIds.BirdShooter))
        };

        var scorePill = new Border
        {
            BackgroundColor = Color.FromArgb("#FFF8E6"), StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 18 },
            Padding = new Thickness(12, 8),
            Content = Text($"⭐ {points}", 16, true, Color.FromArgb("#A66B08"))
        };
        var brand = new VerticalStackLayout
        {
            Spacing = 1,
            Children = { Text("KASKROUT", 19, true, Ink), Text("MINI-ARCADE", 10, true, Muted) }
        };
        var header = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
            VerticalOptions = LayoutOptions.Center
        };
        header.Add(brand, 0, 0);
        header.Add(scorePill, 1, 0);
        body.Children.Add(header);

        var featured = games[DateTime.Now.DayOfYear % games.Length];
        var featuredContent = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
            ColumnSpacing = 8
        };
        var featuredText = new VerticalStackLayout
        {
            Spacing = 5,
            Children =
            {
                new Label { Text = F("DÉFI DU JOUR", "DAILY CHALLENGE", "DAGUITDAGING"), FontSize = 11,
                    FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#EDE6FF"), HorizontalTextAlignment = TextAlignment.Start },
                new Label { Text = F("Ton prochain défi t’attend !", "Your next challenge awaits!", "Je volgende uitdaging wacht!"), FontSize = 25, FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White, HorizontalTextAlignment = TextAlignment.Start, LineBreakMode = LineBreakMode.WordWrap },
                new Label { Text = F("Joue, gagne des points et passe au niveau suivant.", "Play, earn points and reach the next level.", "Speel, verdien punten en bereik het volgende niveau."), FontSize = 13,
                    TextColor = Color.FromArgb("#EEEAFE"), HorizontalTextAlignment = TextAlignment.Start }
            }
        };
        featuredContent.Add(featuredText, 0, 0);
        featuredContent.Add(new Label { Text = featured.Icon, FontSize = 48, VerticalOptions = LayoutOptions.Center }, 1, 0);
        var featuredButton = MakeButton(F($"Jouer à {featured.Title}  →", $"Play {featured.Title}  →", $"Speel {featured.Title}  →"), Colors.White, featured.Play, 46);
        featuredButton.TextColor = Color.FromArgb("#5A43C7");
        featuredButton.FontSize = 15;
        featuredButton.CornerRadius = 15;
        featuredButton.HorizontalOptions = LayoutOptions.Start;
        featuredButton.Padding = new Thickness(17, 0);
        var hero = new Border
        {
            Background = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0), EndPoint = new Point(1, 1),
                GradientStops = { new GradientStop(Color.FromArgb("#5942C8"), 0), new GradientStop(Color.FromArgb("#8369F4"), 1) }
            },
            StrokeThickness = 0, StrokeShape = new RoundRectangle { CornerRadius = 28 },
            Padding = new Thickness(19, 18),
            Shadow = new Shadow { Brush = Color.FromArgb("#405942C8"), Offset = new Point(0, 6), Radius = 14, Opacity = 0.30f },
            Content = new VerticalStackLayout { Spacing = 13, Children = { featuredContent, featuredButton } }
        };
        body.Children.Add(hero);

        var level = points / 100 + 1;
        var progress = points % 100;
        var progressBar = new ProgressBar { Progress = progress / 100.0, ProgressColor = Color.FromArgb("#785DEB"),
            BackgroundColor = Color.FromArgb("#E9E6F4"), HeightRequest = 8 };
        var progressLabels = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
        };
        progressLabels.Add(new Label { Text = F($"NIVEAU {level}", $"LEVEL {level}", $"NIVEAU {level}"), FontSize = 11,
            FontAttributes = FontAttributes.Bold, TextColor = Ink, HorizontalTextAlignment = TextAlignment.Start }, 0, 0);
        progressLabels.Add(new Label { Text = F($"{progress} / 100 pts", $"{progress} / 100 pts", $"{progress} / 100 punten"),
            FontSize = 11, TextColor = Muted, HorizontalTextAlignment = TextAlignment.End }, 1, 0);
        var progressPanel = new Border
        {
            BackgroundColor = Colors.White, StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 19 }, Padding = new Thickness(15, 11),
            Content = new VerticalStackLayout
            {
                Spacing = 7,
                Children =
                {
                    progressLabels,
                    progressBar
                }
            }
        };
        body.Children.Add(progressPanel);

        var languageButton = MakeButton(F("🌐  Français", "🌐  English", "🌐  Nederlands"), Colors.White, SelectLanguage, 44);
        languageButton.TextColor = Purple;
        languageButton.FontSize = 14;
        languageButton.BorderColor = Color.FromArgb("#E5E0F5");
        languageButton.BorderWidth = 1;
        languageButton.CornerRadius = 15;
        body.Children.Add(languageButton);

        var titleRow = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
            Margin = new Thickness(1, 3, 1, 0)
        };
        titleRow.Add(new Label { Text = F("CHOISIS TON PROCHAIN DÉFI", "PICK YOUR NEXT CHALLENGE", "KIES JE VOLGENDE UITDAGING"),
            FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = Ink, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center }, 0, 0);
        var gameCountLabel = new Label { Text = F("9 JEUX", "9 GAMES", "9 SPELLEN"), FontSize = 10, FontAttributes = FontAttributes.Bold,
            TextColor = Muted, VerticalTextAlignment = TextAlignment.Center };
        titleRow.Add(gameCountLabel, 1, 0);
        body.Children.Add(titleRow);

        var grid = new Grid { ColumnSpacing = 11, RowSpacing = 11 };
        grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        var activeCategory = "Tous";
        void RenderGames()
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            var visibleGames = games.Where(game => activeCategory == "Tous" || game.Category == activeCategory).ToArray();
            gameCountLabel.Text = F($"{visibleGames.Length} JEUX", $"{visibleGames.Length} GAMES", $"{visibleGames.Length} SPELLEN");
            for (var i = 0; i < (visibleGames.Length + 1) / 2; i++)
                grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            for (var i = 0; i < visibleGames.Length; i++)
            {
                var game = visibleGames[i];
                var cardContent = new VerticalStackLayout
                {
                    Spacing = 5, VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Border
                        {
                            WidthRequest = 43, HeightRequest = 43, HorizontalOptions = LayoutOptions.Center,
                            BackgroundColor = Color.FromArgb(game.Color),
                            StrokeThickness = 0,
                            StrokeShape = new RoundRectangle { CornerRadius = 16 },
                            Content = Text(game.Icon, 24)
                        },
                        Text(game.Title, 14, true),
                        Text(game.Detail, 11, false, Muted)
                    }
                };
                var card = new Border
                {
                    BackgroundColor = Colors.White,
                    Stroke = Color.FromArgb(game.Color), StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = 22 },
                    Padding = new Thickness(10, 12), HeightRequest = 143,
                    Shadow = new Shadow { Brush = Color.FromArgb("#18202743"), Offset = new Point(0, 3), Radius = 8, Opacity = 0.12f },
                    Content = cardContent
                };
                var tap = new TapGestureRecognizer();
                tap.Tapped += async (_, _) =>
                {
                    await card.ScaleToAsync(0.95, 75);
                    await card.ScaleToAsync(1, 130, Easing.SpringOut);
                    game.Play();
                };
                card.GestureRecognizers.Add(tap);
                grid.Add(card, i % 2, i / 2);
            }
        }

        var filters = new HorizontalStackLayout { Spacing = 8, HorizontalOptions = LayoutOptions.Center };
        var categories = new[] { (Key: "Tous", Fr: "Tous", En: "All", Nl: "Alles"), (Key: "Rapide", Fr: "Rapides", En: "Quick", Nl: "Snel"),
            (Key: "Réflexion", Fr: "Réflexion", En: "Brain", Nl: "Denken"), (Key: "Cartes", Fr: "Cartes", En: "Cards", Nl: "Kaarten") };
        foreach (var category in categories)
        {
            var selected = category.Key == activeCategory;
            var chip = new Button
            {
                Text = F(category.Fr, category.En, category.Nl), FontSize = 12, FontAttributes = FontAttributes.Bold,
                TextColor = selected ? Colors.White : Muted, BackgroundColor = selected ? Purple : Colors.White,
                CornerRadius = 16, HeightRequest = 37, Padding = new Thickness(13, 0),
                BorderColor = selected ? Purple : Color.FromArgb("#E7E4EF"), BorderWidth = 1
            };
            chip.Clicked += (_, _) =>
            {
                activeCategory = category.Key;
                foreach (var child in filters.Children.OfType<Button>())
                {
                    var isSelected = child == chip;
                    child.BackgroundColor = isSelected ? Purple : Colors.White;
                    child.TextColor = isSelected ? Colors.White : Muted;
                    child.BorderColor = isSelected ? Purple : Color.FromArgb("#E7E4EF");
                }
                RenderGames();
            };
            filters.Children.Add(chip);
        }
        body.Children.Add(filters);
        body.Children.Add(grid);
        RenderGames();
    }

    void SetBackdrop(string imageFile)
    {
        pageBackdrop.Source = imageFile;
        var isSolitaire = imageFile == "bg_solitaire.jpg";
        pageBackdrop.Opacity = isSolitaire ? 0.98 : 0.88;
        backdropWash.Opacity = isSolitaire ? 0.08 : 0.34;
    }

    void StartPage(string title, string subtitle)
    {
        activeFeedback = null;
        SetBackdrop(title switch
        {
            "Puzzle coulissant" => "bg_puzzle.jpg",
            "Objet manquant" => "bg_missing.jpg",
            "Jeu des paires" => "bg_pairs.jpg",
            "Lance le dé" => "bg_dice.jpg",
            "Réflexe" => "bg_reflex.jpg",
            "Démineur" => "bg_minesweeper.jpg",
            "Pile ou face" => "bg_coin.jpg",
            "Chasse aux oiseaux – rétro" => "bg_reflex.jpg",
            _ => "bg_home.jpg"
        });
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
            "Pile ou face" => "🪙", "Chasse aux oiseaux – rétro" => "🕹️", _ => "✨"
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

    void LaunchGame(string id) => gameCatalog.Launch(id, this);


    VerticalStackLayout IGameUiHost.Body => body;
    Random IGameUiHost.Random => random;
    IGameSettingsStore IGameUiHost.Settings => gameSettings;
    IPointsService IGameUiHost.Points => pointsService;
    string IGameUiHost.Translate(string value) => T(value);
    string IGameUiHost.Format(string french, string english, string dutch) => F(french, english, dutch);
    Label IGameUiHost.CreateText(string value, double size, bool bold, Color? color) => Text(value, size, bold, color);
    Button IGameUiHost.CreateButton(string label, Color color, Action action, double height) => MakeButton(label, color, action, height);
    Border IGameUiHost.CreatePanel(View content, Color? color) => Panel(content, color);
    void IGameUiHost.StartPage(string title, string subtitle) => StartPage(title, subtitle);
    void IGameUiHost.ShowGameFeedback(bool won, string message, bool temporary) => ShowGameFeedback(won, message, temporary);

}
