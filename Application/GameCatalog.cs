namespace Kaskrout;

public sealed class GameCatalog
{
    private readonly IReadOnlyDictionary<string, Func<IGameUiHost, IGameModule>> factories;

    public GameCatalog(IReadOnlyDictionary<string, Func<IGameUiHost, IGameModule>> registrations)
    {
        factories = new Dictionary<string, Func<IGameUiHost, IGameModule>>(registrations);
    }

    public void Launch(string id, IGameUiHost host)
    {
        if (!factories.TryGetValue(id, out var factory))
            throw new ArgumentOutOfRangeException(nameof(id), id, "Unknown game id.");
        factory(host).Launch();
    }

    public static GameCatalog CreateDefault() => new(new Dictionary<string, Func<IGameUiHost, IGameModule>>
    {
        [GameIds.Puzzle] = host => new PuzzleGame(host),
        [GameIds.MissingObject] = host => new MissingObjectGame(host),
        [GameIds.Pairs] = host => new PairsGame(host),
        [GameIds.Dice] = host => new DiceGame(host),
        [GameIds.BirdShooter] = host => new BirdShooterGame(host),
        [GameIds.Snake] = host => new SnakeGame(host),
        [GameIds.Reflex] = host => new ReflexGame(host),
        [GameIds.Minesweeper] = host => new MinesweeperGame(host),
        [GameIds.Coin] = host => new CoinGame(host),
        [GameIds.Adventure] = host => new AdventureGame(host)
    });
}
