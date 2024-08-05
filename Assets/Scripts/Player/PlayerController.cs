using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Vector2GameEventListener), typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement")]

    [Tooltip("Walking speed of character (units / sec)")]
    public float walkSpeed = 5.0f;
    [Tooltip("Amount of time it takes to reach full walk / run speed (sec)")]
    public float speedUpTime = 0.2f;
    [Tooltip("Time it takes for the character to stop when the joystick is released (sec)")]
    public float stopTime = 0.1f;
    // TODO: IMPLEMENT THIS IN TERMS OF TIME BY SCALING WITH TIME.DELTATIME
    [Tooltip("Portion of the rotation between current and desired orientation to apply each physics update"), Range(0, 1.0f)]
    public float rotationRatio = 0.2f;

    [Header("Vertical Movement")]
    [Tooltip("Gravitational Constant (units / sec / sec)")]
    public float gravity = -15;
    [Tooltip("How high to hover at")]
    public float hoverHeight = 0.50f;
    [Tooltip("How long jumping to Hover Height will take")]
    public float hoverRiseTime = 0.25f;
    [Tooltip("How long to hover before landing")]
    public float maxHoverTime = 3.0f;

    private PlayerHorizontalMotion horizontalMotion;
    private PlayerVerticalMotion verticalMotion;
    private CharacterController characterController;


    // called when the Script instance is being loaded
    private void Awake() { characterController = GetComponent<CharacterController>(); }


    // called before Update is called for the first time
    void Start() { 
        horizontalMotion = new PlayerHorizontalMotion(Camera.main);
        verticalMotion = new PlayerVerticalMotion();
    }


    /// <summary>Callback to be triggered when player <c>move</c> input changes</summary>
    public void OnMoveEvent(Vector2 playerInput) { horizontalMotion.UpdatePlayerInput(playerInput); }

    public void OnJumpEvent(bool playerInput) { verticalMotion.UpdateJumpPressed(playerInput); }


    // called on Physics updates
    private void FixedUpdate()
    {
        if (characterController == null) { return; }

        // rotate towards direction of input
        horizontalMotion.RotateTowardsPlayerInput(transform, rotationRatio);
        
        // move character
        // NOTE: must make _one_ call to `characterController.Move()` so that `characterController.isGrounded` will work reliably
        float horizontalSpeed = horizontalMotion.UpdateSpeed(walkSpeed, speedUpTime, stopTime);
        float verticalVelocity = verticalMotion.UpdateVelocity(characterController.isGrounded, gravity, hoverHeight, hoverRiseTime, maxHoverTime);
        characterController.Move(
            Time.deltaTime * (horizontalSpeed * transform.forward + verticalVelocity * Vector3.up)
        );
    }

}
