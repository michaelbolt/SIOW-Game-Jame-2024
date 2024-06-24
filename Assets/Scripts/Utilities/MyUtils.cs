using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility functions.
/// </summary>
public static class MyUtils
{
    /// <summary>
    /// Project the provided <c>objTransform.forward</c> vector onto the World-space X-Z plane.
    /// </summary>
    /// <param name="objTransform"><c>Transform</c> object to project onto X-Z plane.</param>
    /// <returns>
    /// <c>objTransform.forward</c> projected onto the world-space x-z plane and normalized.
    /// </returns>
    public static Vector3 ProjectForwardOntoXZPlane(Transform objTransform)
    {
        Vector3 worldSpaceForward = objTransform.transform.TransformDirection(Vector3.forward);
        worldSpaceForward.y = 0;  // clear y (up/down) component
        worldSpaceForward.Normalize();  // normalize onto x-z plane
        return worldSpaceForward;
    }
}
