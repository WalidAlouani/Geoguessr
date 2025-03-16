using UnityEngine;

public static class IntersectionUtility
{
    public static bool RayPlaneIntersection(
    Vector3 rayOrigin,
    Vector3 rayDirection,
    Vector3 planePoint,
    Vector3 planeNormal,
    out Vector3 intersection)
    {
        intersection = Vector3.zero;

        float denominator = Vector3.Dot(rayDirection, planeNormal);

        if (Mathf.Abs(denominator) < float.Epsilon)
            return false;

        float t = Vector3.Dot(planePoint - rayOrigin, planeNormal) / denominator;

        if (t < 0)
            return false;

        intersection = rayOrigin + t * rayDirection;
        return true;
    }
}
