using UnityEngine;

public interface IPlayerFactory
{
    PlayerController Create(Player playerData, Vector3 initialPosition);
}
