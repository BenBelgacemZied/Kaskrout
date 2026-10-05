namespace Kaskrout;

public class App : Application
{
    public App()
    {
        var language = Preferences.Default.Get(LanguageSelectionPage.PreferenceKey, string.Empty);
        MainPage = !new[] { "fr", "en", "nl" }.Contains(language)
            ? new LanguageSelectionPage()
            : new MainPage(language);
    }
}

public sealed class LanguageSelectionPage : ContentPage
{
    public const string PreferenceKey = "language";

    public LanguageSelectionPage()
    {
        Title = "Kaskrout";
        BackgroundColor = Color.FromArgb("#F7F6FC");

        var content = new VerticalStackLayout
        {
            Padding = new Thickness(24), Spacing = 18,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Label { Text = "🌍", FontSize = 54, HorizontalTextAlignment = TextAlignment.Center },
                new Label { Text = "Choisissez votre langue\nChoose your language\nKies je taal",
                    FontSize = 24, FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#202743"), HorizontalTextAlignment = TextAlignment.Center },
                new Label { Text = "Vous pourrez la changer plus tard.\nYou can change it later.\nJe kunt dit later wijzigen.",
                    FontSize = 14, TextColor = Color.FromArgb("#7D849B"), HorizontalTextAlignment = TextAlignment.Center }
            }
        };

        content.Children.Add(LanguageButton("🇫🇷   Français", "fr"));
        content.Children.Add(LanguageButton("🇬🇧   English", "en"));
        content.Children.Add(LanguageButton("🇳🇱   Nederlands", "nl"));
        Content = new Grid { Children = { content } };
    }

    static Button LanguageButton(string label, string language)
    {
        var button = new Button
        {
            Text = label, FontSize = 19, FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White, BackgroundColor = Color.FromArgb("#715CE8"),
            CornerRadius = 18, HeightRequest = 60
        };
        button.Clicked += (_, _) =>
        {
            Preferences.Default.Set(PreferenceKey, language);
            Application.Current!.MainPage = new MainPage(language);
        };
        return button;
    }
}
