using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Vector2GameEventListener), typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]

    [Tooltip("Walking speed of character (units / sec)")]
    public float walkSpeed = 5.0f;
    [Tooltip("Amount of time it takes to reach full walk / run speed (sec)")]
    public float speedUpTime = 0.2f;
    [Tooltip("Time it takes for the character to stop when the joystick is released (sec)")]
    public float stopTime = 0.1f;
    // TODO: IMPLEMENT THIS IN TERMS OF TIME BY SCALING WITH TIME.DELTATIME
    [Tooltip("Portion of the rotation between current and desired orientation to apply each physics update"), Range(0, 1.0f)]
    public float rotationRatio = 0.2f;
    [Tooltip("Gravitational Constant (units / sec / sec)")]
    public float gravity = -0.3f;

    private float verticalVelocity = 0.0f;

    private PlayerHorizontalMotion horizontalMotion;
    private CharacterController characterController;


    // called when the Script instance is being loaded
    private void Awake() { characterController = GetComponent<CharacterController>(); }


    // called before Update is called for the first time
    void Start() { horizontalMotion = new PlayerHorizontalMotion(Camera.main); }

    /// <summary>Callback to be triggered when player <c>move</c> input changes</summary>
    public void OnMoveEvent(Vector2 playerInput) { horizontalMotion.UpdatePlayerInput(playerInput); }


    // called on Physics updates
    private void FixedUpdate()
    {
        if (characterController == null) { return; }

        // perform horizontal motion
        horizontalMotion.RotateTowardsPlayerInput(transform, rotationRatio);
        float speed = horizontalMotion.UpdateSpeed(walkSpeed, speedUpTime, stopTime);
        characterController.Move(speed * Time.deltaTime * transform.forward);

        /* Always apply gravity, regardless of player input */
        if (characterController.isGrounded && verticalVelocity < 0.0f) verticalVelocity = 0.0f;
        else verticalVelocity += gravity;
        characterController.Move(verticalVelocity * Time.deltaTime * Vector3.up);
    }

}
