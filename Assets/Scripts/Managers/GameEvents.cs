using System;

public static class GameEvents
{
    public static event Action OnDiceRollRequested;

    public static void RequestDiceRoll()
    {
        OnDiceRollRequested?.Invoke();
    }

    public static event Action<int> OnDiceRolled;

    public static void DiceRolled(int steps)
    {
        OnDiceRolled?.Invoke(steps);
    }
}

