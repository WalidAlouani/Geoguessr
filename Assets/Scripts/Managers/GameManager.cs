using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardDataSO _boardData;
    [SerializeField] private PlayerDataList _playersData;
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private Transform _boardTransform;

    [Inject] private BoardManager _boardManager;
    [Inject] private TurnManager _turnManager;
    [Inject] private PlayersManager _playersManager;

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

        _boardManager.Init(_boardData.GetTiles(), _boardTransform);
        _cameraMovement.Init(_boardData.GetBoardCenter());
        _playersManager.Init(_playersData.Entries, _boardManager.GetHomeTilePosition());
        _turnManager.Init(_playersManager, _boardManager);
    }
}
