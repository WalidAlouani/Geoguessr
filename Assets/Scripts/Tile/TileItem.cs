using System;
using UnityEngine;

public class TileItem : MonoBehaviour
{
    [SerializeField] private TileAnimation _animation;

    public int Index { get; private set; }

    public TileEvent OnStopEvent;
    public TileEvent OnReachEvent;

    public void Init(int index)
    {
        Index = index;
        _animation.PlayStartAnimation(index);
    }

    public void TriggerOnStopEvent(Player player, Action onEventComplete)
    {
        OnStopEvent?.Execute(player, onEventComplete);
    }

    public void TriggerOnReachEvent(Player player, Action onEventComplete)
    {
        OnReachEvent?.Execute(player, onEventComplete);
    }
}
