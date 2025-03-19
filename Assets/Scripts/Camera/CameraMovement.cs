using System;
using UnityEngine;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private CameraBoundaries boundaries;
    [SerializeField] private CameraInputHandler inputHandler;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Transform target;

    private Vector3 _targetPosition;
    private Vector3 _offset;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    void Start()
    {
        _targetPosition = transform.position;
        _offset = transform.position;
    }

    public void Init(Vector2 boardCenter)
    {
        boundaries.Init(boardCenter);
    }

    private void OnEnable()
    {
        inputHandler.OnDragUpdateEvent += OnDragUpdate;
        _signalBus.Subscribe<RollDiceSignal>(OnRollDice);
        _signalBus.Subscribe<TurnEndedSignal>(OnTurnEnded);
    }

    private void OnRollDice(RollDiceSignal signal)
    {
        target = signal.Player.Controller.transform;
    }

    private void OnTurnEnded(TurnEndedSignal signal)
    {
        target = null;
    }

    private void OnDisable()
    {
        inputHandler.OnDragUpdateEvent -= OnDragUpdate;
        _signalBus.Unsubscribe<RollDiceSignal>(OnRollDice);
        _signalBus.Unsubscribe<TurnEndedSignal>(OnTurnEnded);
    }

    private void LateUpdate()
    {
        if (target == null)
            _targetPosition = boundaries.ClampToBounds(_targetPosition);
        else
            _targetPosition = target.position + _offset;

        transform.position = Vector3.Lerp(transform.position, _targetPosition, smoothSpeed * Time.deltaTime);
    }

    private void OnDragUpdate(Vector3 worldDelta)
    {
        if (target == null)
            _targetPosition += worldDelta;
    }
}