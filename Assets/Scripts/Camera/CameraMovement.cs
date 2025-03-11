using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    private float moveSpeed = 10f;        // Speed of movement
    private float smoothTime = 0.2f;      // Smooth damping time

    [Header("Bounds Settings")]
    private Vector2 minBounds = new Vector2(-10f, -10f);  // Lower bound
    private Vector2 maxBounds = new Vector2(10f, 10f);    // Upper bound

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 targetForward;
    private Vector3 targetRight;

    private Vector3 previousMousePosition;

    void Start()
    {
        targetPosition = transform.position;

        targetForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;  // Project onto XY plane
        targetRight = new Vector3(transform.right.x, 0f, transform.right.z).normalized;  // Project onto XY plane
    }

    private void Update()
    {
        HandleKeyboardMovement();
        HandleMouseDragMovement();
    }

    void LateUpdate()
    {
        SmoothMove();
    }

    void HandleKeyboardMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down arrow

        Vector3 moveDir = (targetRight * moveX + targetForward * moveZ) * moveSpeed * Time.deltaTime;
        targetPosition += moveDir;
        ClampToBounds();
    }

    void HandleMouseDragMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }

        // While dragging, compute the difference in mouse position
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 deltaScreen = previousMousePosition - currentMousePosition;

            Vector3 moveDir = (targetRight * deltaScreen.x /*/ Screen.width*/ + targetForward * deltaScreen.y /*/ Screen.height*/) * moveSpeed * Time.deltaTime;
            targetPosition += moveDir;

            Debug.Log(deltaScreen);
            ClampToBounds();
            previousMousePosition = currentMousePosition;
        }
    }

    void SmoothMove()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void ClampToBounds()
    {
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
    }

    public void SetBounds(Vector2 newMinBounds, Vector2 newMaxBounds)
    {
        minBounds = newMinBounds;
        maxBounds = newMaxBounds;
        ClampToBounds();
    }
}
