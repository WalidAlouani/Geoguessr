using System;
using UnityEngine;

public class TileItem : MonoBehaviour
{
    [SerializeField] private TileAnimation _animation;

    private int index;
    public void Init(int index)
    {
        this.index = index;
        _animation.PlayStartAnimation(index);
    }

    public void OnTileReached()
    {
        _animation.PlayStepOnAnimation();
    }
}
