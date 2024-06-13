using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private InputActions inputActions;
    private CharacterController characterController;

    // Awake is called when the script is first loaded
    private void Awake()
    {
        inputActions = new InputActions();
        characterController = GetComponent<CharacterController>();

        // store move input when performed
        inputActions.gameplay.move.performed += context =>
        {
            moveInput = context.ReadValue<Vector2>();
            moveAngle = -Vector2.SignedAngle(Vector2.up, moveInput);
            moveMagnitude = moveInput.magnitude;
            //Debug.Log(moveInput + " -> " + moveMagnitude + " @ " + moveAngle);
        };
        inputActions.gameplay.move.canceled += context =>
        {
            moveInput = Vector2.zero;
            moveAngle = 0.0f;
            moveMagnitude = 0.0f;
        };
    }

    private void OnEnable()
    {
        inputActions.gameplay.Enable();
    }

    private void OnDisable()
    {
        inputActions.gameplay.Disable();
    }
    
    private void FixedUpdate()
    {
        if (characterController != null)
        {
            /* If the player is providing input, rotate towards the direction of the 
             * input in camera-space and move in the new direction.
             * - To rotate smoothly, use a Quaternion SLERP function
             * - To move smoothly, use a damping function to approach the target speed
             *   scaled by the player's input magnitude */
            if (moveMagnitude > 0)
            {
                Vector3 directionOfPlayerInput = Quaternion.AngleAxis(moveAngle, Vector3.up) * ProjectForwardOntoXZPlane(myCamera.transform);
                Quaternion orientationOfPlayerInput = Quaternion.LookRotation(directionOfPlayerInput, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, orientationOfPlayerInput, rotationRatio);

                horizontalSpeed = Mathf.SmoothDamp(horizontalSpeed, walkSpeed * moveMagnitude, ref horizontalAcceleration, speedUpTime);
            }
            else
            {
                horizontalSpeed = Mathf.SmoothDamp(horizontalSpeed, 0.0f, ref horizontalAcceleration, stopTime);
            }
            characterController.Move(horizontalSpeed * Time.deltaTime * transform.forward);

            /* Always apply gravity, regardless of player input */
            if (characterController.isGrounded && verticalVelocity < 0.0f) verticalVelocity = 0.0f;
            else verticalVelocity += gravity;
            characterController.Move(verticalVelocity * Vector3.up * Time.deltaTime);
        }
    }

    #region Movement
    [Tooltip("Camera to base movement on")]
    public Camera myCamera;

    [Header("Movement Parameters")]
    [Tooltip("Walking speed of character (units / sec)")]
    public float walkSpeed = 5.0f;
    [Tooltip("Amount of time it takes to reach full walk / run speed (sec)")]
    public float speedUpTime = 0.2f;
    [Tooltip("Time it takes for the character to stop when the joystick is released (sec)")]
    public float stopTime = 0.1f;
    // TODO: IMPLEMENT THIS IN TERMS OF TIME BY SCALING WITH TIME.DELTATIME
    [Tooltip("Portion of the rotatin between curent and desired orientation to apply each physics update"), Range(0, 1.0f)]
    public float rotationRatio = 0.2f;
    [Tooltip("Gravitational Constant (units / sec / sec)")]
    public float gravity = -10.0f;

    // inputActionvalues
    private Vector2 moveInput = Vector2.zero;
    private float moveAngle = 0.0f;
    private float moveMagnitude = 0.0f;

    // motion values
    private float verticalVelocity = 0.0f;
    private float horizontalSpeed = 0.0f;
    private float horizontalAcceleration = 0.0f;

    /// <summary>
    /// Project the provided <c>Transform</c>'s <c>forward</c> vector onto the World-space X-Z plane.
    /// </summary>
    /// <param name="objTransform"><c>Transform</c> object to project onto X-Z plane.</param>
    /// <returns></returns>
    private Vector3 ProjectForwardOntoXZPlane(Transform objTransform)
    {
        // project myCamera.forward into world space
        Vector3 myCameraForward = myCamera.transform.TransformDirection(Vector3.forward);
        myCameraForward.y = 0;  // clear y component
        myCameraForward.Normalize();  // normalize onto x-z plane

        return myCameraForward;
    }
    #endregion

    #region GIZMOS
    /// <summary>
    /// Draws Gizmos in Scene view for debugging
    /// </summary>
    private void OnDrawGizmos()
    {
        DrawStepHeightPlane();
    }

    private void DrawStepHeightPlane()
    {
        if (characterController != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawCube(
                transform.position                                      // Player location (bottom-center of model
                + characterController.center                            // + CharacterController center point -> center of CharacterController mesh
                - new Vector3(0, characterController.height / 2, 0)     // - half of CharacterController height -> bottom of CharacterController mesh
                + new Vector3(0, characterController.stepOffset, 0)     // + stepOffset -> height of step we can "move up" without interaction
                + new Vector3(0, 0.01f / 2, 0),                         // + half of Gizmo cube height -> bottom of gizmo is aligned with stepOffset
                new Vector3(
                    2 * characterController.radius,
                    0.01f,
                    2 * characterController.radius
                )
                );
        }
    }

    #endregion
}
