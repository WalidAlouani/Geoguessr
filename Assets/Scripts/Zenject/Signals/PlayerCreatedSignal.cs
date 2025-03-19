using System.Collections.Generic;

public class PlayersCreatedSignal
{
    public List<IPlayer> Players { get; private set; }

    public PlayersCreatedSignal(List<IPlayer> players)
    {
        Players = players;
    }
}
