using UnityEngine;
using System;

public class CameraInputHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector3 dragOrigin;
    private bool isDragging = false;

    // Define events for drag actions
    public event Action OnDragStartEvent;
    public event Action<Vector3> OnDragUpdateEvent;
    public event Action OnDragEndEvent;

    private Vector3 planePoint = Vector3.zero;
    private Vector3 planeNormal = Vector3.up;

    void Update()
    {
        HandleMouseDragInput();
    }

    void HandleMouseDragInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(mouseScreenPos);

            if (IntersectionUtility.RayPlaneIntersection(ray.origin, ray.direction, planePoint, planeNormal, out Vector3 intersection))
            {
                dragOrigin = intersection;
                isDragging = true;
                OnDragStartEvent?.Invoke(); // Invoke the Drag Start Event (if anyone is subscribed)
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(mouseScreenPos);

            if (IntersectionUtility.RayPlaneIntersection(ray.origin, ray.direction, planePoint, planeNormal, out Vector3 currentIntersection))
            {
                Vector3 worldDelta = dragOrigin - currentIntersection;
                OnDragUpdateEvent?.Invoke(worldDelta);
                dragOrigin = currentIntersection;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            OnDragEndEvent?.Invoke();
        }
    }
}