using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerVerticalMotion
{
    private enum JumpState
    {                                                       // enum for jump state
        DEFAULT, IDLE, JUMPING, FLOATING, RELEASED, LANDED 
    }                               
    private JumpState currentState = JumpState.DEFAULT;     // current state for jumping / floating
    private bool changedState = false;                      // whether currentState changed this update
    private float currentStateStartTime;                    // when we entered the current state

    private bool jumpPressed = false;           // current state of player jump input

    private float verticalVelocity = 0.0f;      // current vertical velocity (direction matters)
    private float gravity = -0.3f;              // current gravity value

    /// <summary>Update player input for jumping.</summary
    public void UpdateJumpPressed(bool playerInput) { jumpPressed = playerInput; }

    /// <summary>Update the current vertical velocity and return its signed-value.</summary>
    /// <param name="isGrounded">whether the player is touching the ground</param>
    /// <param name="defaultGravity">default gravity when not jumping / floating</param>
    /// <param name="hoverHeight">how high to hover</param>
    /// <param name="hoverRiseTime">how long jump to hoverHeight takes</param>
    /// <param name="maxHoverTime">how long to hover before landing</param>
    /// <returns></returns>
    public float UpdateVelocity(bool isGrounded, float defaultGravity, float hoverHeight, float hoverRiseTime, float maxHoverTime)
    {
        AdvanceJumpStateMachine(isGrounded, maxHoverTime);
        SetGravityForJumpState(defaultGravity, hoverHeight, hoverRiseTime, maxHoverTime);

        // update velocity with current gravity
        if (isGrounded && verticalVelocity < 0.0f) verticalVelocity = 0.0f;
        else verticalVelocity += gravity * Time.deltaTime;
        return verticalVelocity;
    }


    /// <summary>Updates the current jump-state per user input and state.</summary>
    /// <param name="isGrounded">Whether the character is currently touching the ground.</param>
    /// <param name="maxHoverTime">Max amount of time that can be spent in Hover state before falling.</param>
    private void AdvanceJumpStateMachine(bool isGrounded, float maxHoverTime)
    {
        JumpState nextState = currentState;

        switch (currentState)
        {
            case JumpState.DEFAULT:
                /// progress from default state to idle       
                nextState = JumpState.IDLE;
                break;

            case JumpState.IDLE:
                /// can jump from idle, if touching ground
                if (jumpPressed && isGrounded) { nextState = JumpState.JUMPING; }
                break;

            case JumpState.JUMPING:
                // can release button to fall faster
                if (!jumpPressed) { nextState = JumpState.RELEASED; }
                // transition to FLOATING at peak of jump 
                else if (verticalVelocity <= 0.0f) { nextState = JumpState.FLOATING; }
                break;

            case JumpState.FLOATING:
                // can release button to fall faster
                // OR exceed maxHovertime
                if (!jumpPressed || (Time.time - currentStateStartTime) > maxHoverTime) { nextState = JumpState.RELEASED; }
                // check for landing
                goto case JumpState.RELEASED;
            case JumpState.RELEASED:
                if (isGrounded) { nextState = JumpState.LANDED; }
                break;

            case JumpState.LANDED:
                // return to IDLE by releasing the button; prevents double jumping
                if (!jumpPressed) { nextState = JumpState.IDLE; }
                break;

            default:
                //handle error
                Debug.LogError("Unsupported JumpState: " + currentState.ToString());
                break;
        }

        changedState = nextState != currentState;
        if (changedState) {
            currentStateStartTime = Time.time; 
        }
        currentState = nextState;
    }

    /// <summary>Ensure that gravity is correctly set for the current jump-state.</summary>
    /// <remarks>
    /// <para>
    /// In the presence of constant gravity, the current height can be calculated as
    /// <c>h(t) = g * t^2 + v(t) * t</c> and current velocity as 
    /// <c>v(t) = v_0 + 2 * g * t</c>.
    /// </para>
    /// <para>
    /// From this, gravity and initial velocity can be chosen to reach a target height
    /// <c>H</c> at time <c>T</c>:
    /// <list type="bullet">
    /// <item><c>v_0 = 2 * H / T</c></item>
    /// <item><c>g = -2 * H / T^2</c></item>
    /// </list>
    /// Because motion is symmetric, the same gravity <c>g</c> will result in 
    /// <em>falling</em> a distance of <c>H</c> in time <c>T</c> if the initial velocity
    /// <c>v_0</c>is set to 0.
    /// </para>
    /// </remarks>
    private void SetGravityForJumpState(float defaultGravity, float hoverHeight, float hoverRiseTime, float maxHoverTime)
    {
        switch (currentState)
        {
            default:
            case JumpState.IDLE:
            case JumpState.LANDED:
                gravity = defaultGravity;
                break;

            case JumpState.JUMPING:
                // set initial velocity if we _just_ entered this state
                if (changedState) { verticalVelocity = 2 * hoverHeight / hoverRiseTime; };
                goto case JumpState.RELEASED;
            case JumpState.RELEASED:
                gravity = -2 * hoverHeight / (hoverRiseTime * hoverRiseTime);
                break;

            case JumpState.FLOATING:
                // set velocity to 0 to ensure we float down slow enough
                if (changedState) { verticalVelocity = 0.0f; };
                gravity = -2 * hoverHeight / (maxHoverTime * maxHoverTime);
                break;
        }
    }

}
