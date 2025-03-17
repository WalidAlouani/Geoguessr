using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardDataSO _boardData;
    [SerializeField] private PlayerDataList _playersData;
    [SerializeField] private PlayersManager _playersManager;
    [SerializeField] private BoardManager _boardManager;
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private CameraMovement _cameraMovement;

    private void Start()
    {
        if (_boardData == null)
        {
            Debug.LogError("BoardData is not assigned!");
            return;
        }

        if (_playersData == null || _playersData.Entries.Count == 0)
        {
            Debug.LogError("PlayersData is missing or empty!");
            return;
        }

        _boardManager.Init(_boardData.GetTiles());
        _cameraMovement.Init(_boardData.GetBoardCenter());
        _playersManager.Init(_playersData.Entries, _boardManager.GetHomeTilePosition());
        _turnManager.Init(_playersManager, _boardManager);
    }
}
