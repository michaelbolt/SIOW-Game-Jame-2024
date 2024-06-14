using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region UNITY_LIFECYCLE
    // called when script instance is first loaded
    private void Awake()
    {
        OnAwakeGetCharacterController();
        OnAwakeBindInputs();
    }

    // called when GameObject is enabled
    private void OnEnable()
    {
        inputActions.gameplay.Enable();
    }

    // called when GameObject is disabled
    private void OnDisable()
    {
        inputActions.gameplay.Disable();
    }
    
    // called on every Physics update
    private void FixedUpdate()
    {
        ApplyMotion();
    }
    #endregion

    #region PLAYER_INPUT
    private InputActions inputActions;
    private Vector2 moveInput = Vector2.zero;
    private float moveAngle = 0.0f;
    private float moveMagnitude = 0.0f;

    /// <summary>
    /// Instantiate the <c>InputActions</c> class and bind callbacks to store the 
    /// current state of player input on changes.
    /// </summary>
    /// /// <remarks>Must be called during <c>OnAwake()</c></remarks>
    private void OnAwakeBindInputs()
    {
        inputActions = new InputActions();
        inputActions.gameplay.move.performed += context =>
        {
            moveInput = context.ReadValue<Vector2>();
            moveAngle = -Vector2.SignedAngle(Vector2.up, moveInput);
            moveMagnitude = moveInput.magnitude;
        };
        inputActions.gameplay.move.canceled += context =>
        {
            moveInput = Vector2.zero;
            moveAngle = 0.0f;
            moveMagnitude = 0.0f;
        };
    }

    #endregion

    #region PLAYER_MOVEMENT
    [Header("Player Movement")]

    [Tooltip("Camera to perform player motion relative to")]
    public Camera myCamera;
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

    private CharacterController characterController;
    private float verticalVelocity = 0.0f;
    private float horizontalSpeed = 0.0f;
    private float horizontalAcceleration = 0.0f;

    /// <summary>
    /// Get a reference to the GameObject's <c>Charactercontroller</c> <c>Component</c>
    /// for use
    /// </summary>
    /// <remarks>Must be called during <c>OnAwake()</c></remarks>
    private void OnAwakeGetCharacterController()
    {
        characterController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Apply motion to the GameObject based on player input, the current camera 
    /// orientation, and gravity.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// Motion is performed by first rotating the object towards the direction of player 
    /// input (relative to the current camera view) <em>then</em> moving along that 
    /// direction.
    /// </item>
    /// <item>
    /// Rotation is achieved by using a spherical linear-interpolation (SLERP) between 
    /// the current object orientation Quaternion and the "dired direction of travel"
    /// Quaternion.
    /// </item>
    /// <item>
    /// Forward motion is achieved using a smooth-damping function to approach the 
    /// maximum walking speed scaled by the player's input.
    /// </item>
    /// </list>
    /// </remarks>
    private void ApplyMotion()
    {
        if (characterController != null)
        {
            if (moveMagnitude > 0)
            {
                Vector3 directionOfPlayerInput = Quaternion.AngleAxis(moveAngle, Vector3.up) * MyUtils.ProjectForwardOntoXZPlane(myCamera.transform);
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
