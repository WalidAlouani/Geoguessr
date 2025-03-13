using UnityEngine;

public class CameraDragMovement : MonoBehaviour
{
    [SerializeField] private CameraBoundaries boundaries;
    [SerializeField] private CameraInputHandler inputHandler;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Transform Target;

    private Vector3 targetPosition;
    private Vector3 offset;

    void Start()
    {
        targetPosition = transform.position;
        offset = transform.position;
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