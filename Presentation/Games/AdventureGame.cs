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
        body.Children.Add(Panel(new VerticalStackLayout
        {
            Spacing = 10,
            Children =
            {
                Text("🏡", 64, true),
                Text("Construis ton village", 22, true),
                Text("Joue aux mini-jeux pour gagner des pièces, puis utilise-les ici.", 15, false, Muted),
                budget
            }
        }, Color.FromArgb("#F0F8EA")));

        foreach (var building in Buildings)
        {
            var settingKey = $"adventure-built-{building.Id}";
            var isBuilt = GetIntSetting(settingKey) == 1;
            Button button = null!;
            button = MakeButton(
                isBuilt ? "Déjà construit ✓" : $"{T("Construire")} · {building.Cost} 💰",
                isBuilt ? Green : Purple,
                () =>
                {
                    if (GetIntSetting(settingKey) == 1) return;
                    if (!Wallet.TrySpend(building.Cost))
                    {
                        var missing = building.Cost - Wallet.Balance;
                        status.Text = $"{T("Il te manque")} {missing} {T("pièces. Joue à un mini-jeu pour en gagner.")}";
                        ShowGameFeedback(false, $"{T("Il te manque")} {missing} {T("pièces. Joue à un mini-jeu pour en gagner.")}", true);
                        return;
                    }

                    SetIntSetting(settingKey, 1);
                    budget.Text = $"{T("Budget")} : {Wallet.Balance} 💰";
                    button.Text = T("Déjà construit ✓");
                    button.BackgroundColor = Green;
                    button.IsEnabled = false;
                    status.Text = $"{T(building.Name)} — {T("CONSTRUCTION TERMINÉE !")}";
                    ShowGameFeedback(true, status.Text);
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
