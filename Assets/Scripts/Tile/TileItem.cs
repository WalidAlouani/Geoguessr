using System;
using UnityEngine;
using Zenject;

public class TileItem : MonoBehaviour
{
    [SerializeField] private TileAnimation _animation;

    public int Index { get; private set; }

    public TileEvent OnStopEvent;
    public TileEvent OnReachEvent;

    protected SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Init(int index)
    {
        Index = index;
        _animation.PlayStartAnimation(index);
    }

    public void TriggerOnStopEvent(Player player)
    {
        OnStopEvent?.Execute(this, player, _signalBus);
    }

    public void TriggerOnReachEvent(Player player)
    {
        _animation.PlayStepOnAnimation();
        OnReachEvent?.Execute(this, player, _signalBus);
    }
}
