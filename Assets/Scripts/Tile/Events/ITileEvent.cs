using System;

public interface ITileEvent
{
    void Execute(Player player, Action onEventComplete);
}