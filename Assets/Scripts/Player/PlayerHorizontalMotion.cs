using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This class manages the player's horizontal motion (direction facing + speed).</summary>
public class PlayerHorizontalMotion
{
    private readonly Camera mainCamera;

    private float moveAngle = 0.0f;
    private float moveMagnitude = 0.0f;

    private float currentSpeed = 0.0f;
    private float currentAcceleration = 0.0f;

    /// <summary>Construct a new <c>PlayerHorizontalMotion</c> object.</summary>
    /// <param name="mainCamera"><c>Camera</c> to base movement relative to.</param>
    public PlayerHorizontalMotion(Camera mainCamera) 
    { 
        this.mainCamera = mainCamera;
    }

    /// <summary>Update player input for movement control.</summary>
    public void UpdatePlayerInput(Vector2 playerInput)
    {
        moveAngle = -Vector2.SignedAngle(Vector2.up, playerInput);
        moveMagnitude = playerInput.magnitude;
    }

    /// <summary>
    /// Rotate the provided <c>Transform</c> towards the direction of player input 
    /// (relative to the <c>mainCamera</c>) by a given amount.
    /// </summary>
    /// <param name="objTransform">Object to rotate towards player input direction</param>
    /// <param name="by">Amount to rotate by [0, 1]</param>
    public void RotateTowardsPlayerInput(Transform objTransform, float by)
    {
        if (moveMagnitude == 0.0f) { return;  }  // do not rotate if no player input
        
        Vector3 cameraForward_xz = ProjectCameraForwardOntoXZPlane();
        Vector3 playerInputDirection = Quaternion.AngleAxis(moveAngle, Vector3.up) * cameraForward_xz;
        Quaternion targetOrientation = Quaternion.LookRotation(playerInputDirection, Vector3.up);

        objTransform.rotation = Quaternion.Slerp(objTransform.rotation, targetOrientation, by);
    }

    /// <summary>Smoothly update the current speed.</summary>
    /// <param name="maxSpeed">Max speed to accelerate towards; will be scaled by player input.</param>
    /// <param name="accelerationTime">Time to spend accelerating to <c>maxSpeed</c>.</param>
    /// <param name="stopTime">Time to spend decelerating if no player input.</param>
    /// <returns></returns>
    public float UpdateSpeed(float maxSpeed, float accelerationTime, float stopTime)
    {
        currentSpeed = Mathf.SmoothDamp(
            currentSpeed,
            maxSpeed * moveMagnitude,
            ref currentAcceleration,
            moveMagnitude == 0.0f ? stopTime : accelerationTime
        );
        return currentSpeed;
    }


    /// <summary>Returns the <c>mainCamera.forward</c> vector transformed onto the World-space X-Z plane.</summary>
    private Vector3 ProjectCameraForwardOntoXZPlane()
    {
        Vector3 worldSpaceForward = mainCamera.transform.TransformDirection(Vector3.forward);
        worldSpaceForward.y = 0;  // clear y (up/down) component
        worldSpaceForward.Normalize();  // normalize onto x-z plane
        return worldSpaceForward;
    }
}
