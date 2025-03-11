using UnityEngine;

public static class IntersectionUtility
{
    /// <summary>
    /// Calculates the intersection of a ray and a plane.
    /// </summary>
    /// <param name="rayOrigin">The starting point of the ray.</param>
    /// <param name="rayDirection">The direction of the ray (should be normalized).</param>
    /// <param name="planePoint">A point on the plane.</param>
    /// <param name="planeNormal">The normal vector of the plane (should be normalized).</param>
    /// <param name="intersection">The intersection point, if one exists.</param>
    /// <returns>True if the intersection exists in the ray's forward direction, false otherwise.</returns>
    public static bool RayPlaneIntersection(
        Vector3 rayOrigin,
        Vector3 rayDirection,
        Vector3 planePoint,
        Vector3 planeNormal,
        out Vector3 intersection)
    {
        intersection = Vector3.zero;

        // Compute the dot product of the ray direction and the plane normal.
        float denominator = Vector3.Dot(rayDirection, planeNormal);

        // If the denominator is nearly zero, the ray is parallel to the plane.
        if (Mathf.Abs(denominator) < float.Epsilon)
            return false;

        // Calculate t: the parameter along the ray at which the intersection occurs.
        float t = Vector3.Dot(planePoint - rayOrigin, planeNormal) / denominator;

        // If t is negative, the intersection is behind the ray origin.
        if (t < 0)
            return false;

        // Compute the intersection point.
        intersection = rayOrigin + t * rayDirection;
        return true;
    }
}
