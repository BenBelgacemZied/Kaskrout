using Microsoft.Maui.Controls.Shapes;

namespace Kaskrout;

public sealed class AdventureGame(IGameUiHost host) : GameModuleBase(host)
{
    private sealed record Building(string Id, string Name, string Description, string Emoji, int Cost);

    private static readonly Building[] Buildings =
    [
        new("house", "Maison", "Un premier toit pour ton village.", "🏠", 300),
        new("garden", "Jardin", "Un coin de nature pour te détendre.", "🌿", 150),
        new("workshop", "Atelier", "Un atelier pour développer ton village.", "🛠️", 700)
    ];

    public override string Id => GameIds.Adventure;

    public override void Launch()
    {
        StartPage("Aventure", "Construis ton village avec les pièces gagnées dans les mini-jeux.");

        var budget = Text($"{T("Budget")} : {Wallet.Balance} 💰", 21, true, Color.FromArgb("#A66B08"));
        var status = Text("CHOISIS UNE CONSTRUCTION POUR COMMENCER.", 15, false, Muted);
        var plots = new Grid { ColumnSpacing = 7, HeightRequest = 136 };
        for (var i = 0; i < Buildings.Length; i++)
            plots.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        var scene = new Border
        {
            Background = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops =
                {
                    new GradientStop(Color.FromArgb("#C9E8A9"), 0),
                    new GradientStop(Color.FromArgb("#82C879"), 1)
                }
            },
            Stroke = Color.FromArgb("#5E9B5A"),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 24 },
            Padding = new Thickness(10),
            Content = new VerticalStackLayout
            {
                Spacing = 7,
                Children =
                {
                    new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition(GridLength.Star),
                            new ColumnDefinition(GridLength.Auto)
                        },
                        Children =
                        {
                            { new Label { Text = "🌳  🌲", FontSize = 22, VerticalTextAlignment = TextAlignment.Center }, 0, 0 },
                            { new Label { Text = "☀️", FontSize = 22, HorizontalTextAlignment = TextAlignment.End }, 1, 0 }
                        }
                    },
                    plots,
                    new Label
                    {
                        Text = "🪨    🌱    🪨    🌱    🪨",
                        FontSize = 17,
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
            }
        };

        body.Children.Add(Panel(new VerticalStackLayout
        {
            Spacing = 9,
            Children =
            {
                Text("Construis ton village", 22, true),
                Text("Joue aux mini-jeux pour gagner des pièces, puis utilise-les ici.", 15, false, Muted),
                budget
            }
        }, Color.FromArgb("#F0F8EA")));
        body.Children.Add(scene);

        void RenderPlots()
        {
            plots.Children.Clear();
            for (var i = 0; i < Buildings.Length; i++)
            {
                var building = Buildings[i];
                var built = GetIntSetting($"adventure-built-{building.Id}") == 1;
                var plotContent = new VerticalStackLayout
                {
                    Spacing = 3,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };

                if (building.Id == "garden" && built)
                {
                    plotContent.Children.Add(new Label
                    {
                        Text = "🌷  🌼",
                        FontSize = 22,
                        HorizontalTextAlignment = TextAlignment.Center
                    });
                    plotContent.Children.Add(new Label
                    {
                        Text = "🌿  🌻",
                        FontSize = 22,
                        HorizontalTextAlignment = TextAlignment.Center
                    });
                }
                else
                {
                    plotContent.Children.Add(new Label
                    {
                        Text = built ? building.Emoji : "🌱",
                        FontSize = 30,
                        HorizontalTextAlignment = TextAlignment.Center
                    });
                }

                plotContent.Children.Add(new Label
                {
                    Text = T(built ? building.Name : "Parcelle libre"),
                    FontSize = 12,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Ink,
                    HorizontalTextAlignment = TextAlignment.Center
                });

                plots.Add(new Border
                {
                    BackgroundColor = built ? Color.FromArgb("#E6F5D8") : Color.FromArgb("#CBE2AA"),
                    Stroke = built ? Color.FromArgb("#6EAA63") : Color.FromArgb("#A7C68A"),
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = 15 },
                    Padding = new Thickness(3),
                    Content = plotContent
                }, i, 0);
            }
        }

        RenderPlots();

        foreach (var building in Buildings)
        {
            var settingKey = $"adventure-built-{building.Id}";
            var isBuilt = GetIntSetting(settingKey) == 1;
            Button button = null!;
            button = MakeButton(
                isBuilt ? "Déjà construit ✓" : $"{T("Construire")} · {building.Cost} 💰",
                isBuilt ? Green : Purple,
                async () =>
                {
                    if (GetIntSetting(settingKey) == 1) return;
                    if (!Wallet.TrySpend(building.Cost))
                    {
                        var missing = building.Cost - Wallet.Balance;
                        status.Text = $"{T("Il te manque")} {missing} {T("pièces. Joue à un mini-jeu pour en gagner.")}";
                        ShowGameFeedback(false, status.Text, true);
                        return;
                    }

                    SetIntSetting(settingKey, 1);
                    budget.Text = $"{T("Budget")} : {Wallet.Balance} 💰";
                    button.Text = T("Déjà construit ✓");
                    button.BackgroundColor = Green;
                    button.IsEnabled = false;
                    RenderPlots();
                    status.Text = $"{T(building.Name)} — {T("CONSTRUCTION TERMINÉE !")}";
                    ShowGameFeedback(true, status.Text);
                    await scene.ScaleToAsync(1.025, 110, Easing.CubicOut);
                    await scene.ScaleToAsync(1, 150, Easing.SpringOut);
                },
                54);
            if (isBuilt) button.IsEnabled = false;

            body.Children.Add(Panel(new VerticalStackLayout
            {
                Spacing = 5,
                Children =
                {
                    Text($"{building.Emoji}  {building.Name}", 20, true, Ink),
                    Text(building.Description, 14, false, Muted),
                    button
                }
            }, Colors.White));
        }

        body.Children.Add(status);
    }
}
