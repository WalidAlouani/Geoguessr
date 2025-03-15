public class RollDiceSignal { }
public class DiceRolledSignal
{
    public int Steps { get; private set; }

    public DiceRolledSignal(int steps)
    {
        Steps = steps;
    }
}
