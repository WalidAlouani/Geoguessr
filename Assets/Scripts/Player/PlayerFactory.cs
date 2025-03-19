using Zenject;

public static class PlayerFactory
{
    public static IPlayer Create(PlayerType type, int index, string name, int coins, SignalBus signalBus)
    {
        switch (type)
        {
            case PlayerType.Humain:
                return new PlayerHumain(type, index, name, coins, signalBus);
            case PlayerType.AI:
                return new PlayerAI(type, index, name, coins, signalBus);
            default:
                return new PlayerHumain(type, index, name, coins, signalBus);
        }
    }
}
