using UnityEngine;

public interface IPlayerFactory
{
    PlayerController Create(IPlayer playerData, Vector3 initialPosition);
}
