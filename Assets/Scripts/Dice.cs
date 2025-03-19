using UnityEngine;
using Zenject;

public class Dice
{
    private readonly int _minValue;
    private readonly int _maxValue;
    private readonly SignalBus _signalBus;

    [Inject]
    public Dice(int minValue, int maxValue, SignalBus signalBus)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        _signalBus = signalBus;
    }

    public int Roll()
    {
        var value = Random.Range(_minValue, _maxValue + 1);
        _signalBus.Fire(new DiceRolledSignal(value));

        return value;
    }
}

public class DiceFactory : PlaceholderFactory<int, int, Dice>
{
}