public interface IPlayer
{
    public int Index { get; }
    public string Name { get; }
    public PlayerType Type { get; }
    public int Coins { get; }
    public PlayerController Controller { get; }

    void SetController(PlayerController controller);
    void TurnStarted();
    void TurnEnded();
    void AddCoins(int coinAmount);
    void Dispose();
}
