using UnityEngine;

public class DiceService : IDiceService
{
    public int RollDice()
    {
        return Random.Range(0, 11);
    }
}
