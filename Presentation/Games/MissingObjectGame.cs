namespace Kaskrout;

public sealed class MissingObjectGame(IGameUiHost host) : GameModuleBase(host)
{
    public override string Id => GameIds.MissingObject;

    public override void Launch()
    {
        StartPage("Objet manquant", "Observe les images, puis retrouve celle qui a disparu.");
        var objectBank = new[]
        {
            "🍎","🍐","🍊","🍋","🍉","🍇","🍓","🫐","🍒","🍑","🥝","🍍","🥭","🍌","🥥","🥑",
            "🥕","🌽","🥦","🍅","🥒","🫑","🍄","🥔","🍞","🥐","🧀","🍕","🍔","🍟","🌭","🍿",
            "🍩","🍪","🎂","🍦","🍭","⚽","🏀","🏈","🎾","🏓","🏆","🎸","🎹","🎺","🥁","🎻",
            "🚗","🚕","🚌","🚎","🚓","🚑","🚒","🚲","🛴","🚜","🚂","✈️","🚀","🚁","⛵","🚢",
            "🐶","🐱","🐭","🐹","🐰","🦊","🐻","🐼","🐨","🐯","🦁","🐮","🐷","🐸","🐵","🐧",
            "🐦","🦆","🦉","🦋","🐝","🐢","🐙","🦀","🐬","🐳","🌻","🌵","🌈","☀️","🌙","⭐",
            "🎈","🎁","🧸","📚","✏️","🎒","⌚","🔑","📷","💡","🪁","🧩","🎲","👑","🕶️"
        };
        var targetOrder = objectBank.OrderBy(_ => random.Next()).ToArray();
        var totalLevels = 10;
        var questionsPerLevel = 3;
        var totalQuestions = totalLevels * questionsPerLevel;
        var questionIndex = 0;
        var score = 0;
        var levelCorrect = 0;
        var roundArea = new VerticalStackLayout { Spacing = 12 };
        var levelLabel = Text("", 15, true, Purple);
        var progressBar = new ProgressBar
        {
            Progress = 0, ProgressColor = Green, BackgroundColor = Color.FromArgb("#E5E8F0"),
            HeightRequest = 8
        };
        var scoreLabel = Text(F("Bonnes réponses : 0", "Correct answers: 0", "Goede antwoorden: 0"), 14, true, Muted);
        body.Children.Add(levelLabel);
        body.Children.Add(progressBar);
        body.Children.Add(scoreLabel);
        body.Children.Add(roundArea);

        void UpdateProgress()
        {
            var displayLevel = Math.Min(totalLevels, questionIndex / questionsPerLevel + 1);
            var questionInLevel = questionIndex % questionsPerLevel + 1;
            levelLabel.Text = F(
                $"NIVEAU {displayLevel} / {totalLevels}  •  QUESTION {questionInLevel} / {questionsPerLevel}",
                $"LEVEL {displayLevel} / {totalLevels}  •  QUESTION {questionInLevel} / {questionsPerLevel}",
                $"NIVEAU {displayLevel} / {totalLevels}  •  VRAAG {questionInLevel} / {questionsPerLevel}");
            progressBar.Progress = questionIndex / (double)totalQuestions;
            scoreLabel.Text = F($"Bonnes réponses : {score}", $"Correct answers: {score}", $"Goede antwoorden: {score}");
        }

        void ShowRound()
        {
            if (!body.Children.Contains(roundArea)) return;
            roundArea.Children.Clear();
            if (questionIndex >= totalQuestions)
            {
                levelLabel.Text = F("DÉFI TERMINÉ !", "CHALLENGE COMPLETE!", "UITDAGING VOLTOOID!");
                progressBar.Progress = 1;
                roundArea.Children.Add(Panel(new VerticalStackLayout
                {
                    Spacing = 12,
                    Children =
                    {
                        Text("Bien joué !", 25, true, Green),
                        Text(F($"Tu as trouvé {score} objets sur {totalQuestions}.", $"You found {score} objects out of {totalQuestions}.", $"Je vond {score} voorwerpen van de {totalQuestions}."), 17)
                    }
                }));
                roundArea.Children.Add(MakeButton("Rejouer", Purple, Launch));
                AddPoints(score * 2);
                ShowGameFeedback(true, F($"Défi terminé ! +{score * 2} points", $"Challenge complete! +{score * 2} points", $"Uitdaging voltooid! +{score * 2} punten"));
                return;
            }

            UpdateProgress();
            var level = questionIndex / questionsPerLevel + 1;
            if (questionIndex % questionsPerLevel == 0) levelCorrect = 0;
            var shownCount = Math.Min(8, 4 + (level - 1) / 2);
            var optionCount = Math.Min(6, 4 + (level - 1) / 3);
            var missing = targetOrder[questionIndex];
            var visibleItems = objectBank.Where(item => item != missing)
                .OrderBy(_ => random.Next()).Take(shownCount - 1)
                .Append(missing).OrderBy(_ => random.Next()).ToArray();
            var visible = Text(string.Join("   ", visibleItems), level >= 7 ? 27 : 31);
            var question = Text("Mémorise bien…", 16, true, Purple);
            var panel = Panel(new VerticalStackLayout { Spacing = 12, Children = { visible, question } });
            roundArea.Children.Add(panel);
            panel.Scale = 0.96;
            _ = panel.ScaleToAsync(1, 220, Easing.SpringOut);

            var optionsArea = new Grid { RowSpacing = 8, ColumnSpacing = 8 };
            optionsArea.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            optionsArea.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            optionsArea.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            optionsArea.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            roundArea.Children.Add(optionsArea);

            var memorizeDelay = Math.Max(900, 2200 - (level - 1) * 130);
            Task.Delay(memorizeDelay).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!body.Children.Contains(roundArea) || !roundArea.Children.Contains(panel)) return;
                visible.Text = string.Join("   ", visibleItems.Where(item => item != missing));
                question.Text = T("Lequel a disparu ?");
                var decoys = objectBank.Where(item => !visibleItems.Contains(item))
                    .OrderBy(_ => random.Next()).Take(optionCount - 1);
                var options = decoys.Append(missing).OrderBy(_ => random.Next()).ToArray();
                for (var i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    var answer = MakeButton(option, Colors.White, () =>
                    {
                        if (optionsArea.Children.All(view => view is not Button button || !button.IsEnabled)) return;
                        foreach (var child in optionsArea.Children)
                            if (child is Button button) button.IsEnabled = false;

                        if (option == missing)
                        {
                            score++;
                            levelCorrect++;
                            question.Text = F("Exactement ! ✨", "That's right! ✨", "Precies! ✨");
                            question.TextColor = Green;
                        }
                        else
                        {
                            question.Text = F($"C’était {missing} !", $"It was {missing}!", $"Het was {missing}!");
                            question.TextColor = Color.FromArgb("#D94D68");
                        }

                        questionIndex++;
                        var justCompletedLevel = questionIndex < totalQuestions && questionIndex % questionsPerLevel == 0;
                        if (justCompletedLevel)
                            question.Text = F(
                                $"Niveau terminé : {levelCorrect}/{questionsPerLevel} bonnes réponses !",
                                $"Level complete: {levelCorrect}/{questionsPerLevel} correct answers!",
                                $"Niveau voltooid: {levelCorrect}/{questionsPerLevel} goede antwoorden!");

                        Task.Delay(850).ContinueWith(__ => MainThread.BeginInvokeOnMainThread(ShowRound));
                    }, 68);
                    answer.FontSize = 32;
                    answer.CornerRadius = 18;
                    answer.Shadow = new Shadow { Brush = Color.FromArgb("#24715CE8"), Offset = new Point(0, 3), Radius = 7, Opacity = 0.22f };
                    optionsArea.Add(answer, i % 2, i / 2);
                }
            }));
        }

        ShowRound();
    }
}
