using UnityEngine;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private CameraBoundaries boundaries;
    [SerializeField] private CameraInputHandler inputHandler;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Transform Target;

    private Vector3 targetPosition;
    private Vector3 offset;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    void Start()
    {
        targetPosition = transform.position;
        offset = transform.position;
    }

    private void OnEnable()
    {
        inputHandler.OnDragUpdateEvent += OnDragUpdate;
        _signalBus.Subscribe<PlayerStartMoveSignal>(OnTurnStarted);
        _signalBus.Subscribe<TurnEndedSignal>(OnTurnEnded);
    }

    private void OnTurnStarted(PlayerStartMoveSignal signal)
    {
        Target = signal.Player.Controller.transform;
    }

    private void OnTurnEnded(TurnEndedSignal signal)
    {
        Target = null;
    }

    private void OnDisable()
    {
        inputHandler.OnDragUpdateEvent -= OnDragUpdate;
        _signalBus.Unsubscribe<PlayerStartMoveSignal>(OnTurnStarted);
        _signalBus.Unsubscribe<TurnEndedSignal>(OnTurnEnded);
    }

    private void LateUpdate()
    {
        if (Target == null)
            targetPosition = boundaries.ClampToBounds(targetPosition);
        else
            targetPosition = Target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    private void OnDragUpdate(Vector3 worldDelta)
    {
        if (Target == null)
            targetPosition += worldDelta;
    }
}