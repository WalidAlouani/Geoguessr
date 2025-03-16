using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardDataSO boardData;
    [SerializeField] private PlayersManager playersManager;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private CameraBoundaries cameraBoundaries;

    private void Start()
    {
        boardManager.Init(boardData.GetTiles());
        cameraBoundaries.Init(boardData.GetBoardCenter());
        playersManager.Init(boardManager.GetHomeTilePosition());
        turnManager.Init(playersManager, boardManager);
    }
}
