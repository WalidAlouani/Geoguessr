using UnityEngine;

public interface IPlayerFactory
{
    PlayerController Create(PlayerData playerData, Vector3 initialPosition, int index);
}
