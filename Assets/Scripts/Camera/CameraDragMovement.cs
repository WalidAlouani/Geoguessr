using UnityEngine;

public class CameraDragMovement : MonoBehaviour
{
    [SerializeField] private CameraBoundaries boundaries;
    [SerializeField] private CameraInputHandler inputHandler;
    [SerializeField] private float smoothSpeed = 10f;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    private void OnEnable()
    {
        inputHandler.OnDragUpdateEvent += OnDragUpdate;
    }

    private void OnDisable()
    {
        inputHandler.OnDragUpdateEvent -= OnDragUpdate;
    }

    private void LateUpdate()
    {
        targetPosition = boundaries.ClampToBounds(targetPosition);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    private void OnDragUpdate(Vector3 worldDelta)
    {
        targetPosition += worldDelta;
    }
}