using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    [SerializeField] private Vector2 BoundsX = new Vector2(-10f, 10);
    [SerializeField] private Vector2 BoundsZ = new Vector3(-10f, 10);

    public void Init(Vector2 center)
    {
        transform.position = new Vector3(center.x * 0.5f, 0, center.y * 0.5f);
        BoundsX = new Vector2(center.x, -center.x);
        BoundsZ = new Vector2(-center.y, center.y);
    }

    public Vector3 ClampToBounds(Vector3 worldPos)
    {
        // Convert this offset into the object's local space.
        Vector3 localOffset = transform.InverseTransformVector(worldPos);

        // Clamp the x and y components.
        localOffset.x = Mathf.Clamp(localOffset.x, BoundsX.x, BoundsX.y);
        localOffset.z = Mathf.Clamp(localOffset.z, BoundsZ.x, BoundsZ.y);

        // Convert the clamped local offset back to world space.
        Vector3 clampedOffset = transform.TransformVector(localOffset);
        return new Vector3(clampedOffset.x, worldPos.y, clampedOffset.z);
    }
}
