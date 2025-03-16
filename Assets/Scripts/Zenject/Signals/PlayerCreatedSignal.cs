using System.Collections.Generic;

public class PlayersCreatedSignal
{
    public List<Player> Players { get; private set; }

    public PlayersCreatedSignal(List<Player> players)
    {
        Players = players;
    }
}
