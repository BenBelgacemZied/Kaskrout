namespace Kaskrout;

public sealed class CoinGame(IGameUiHost host) : GameModuleBase(host)
{
    public override string Id => GameIds.Coin;

    public override void Launch()
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
}
