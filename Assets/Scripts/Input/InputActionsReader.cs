// Ignore Spelling: gameplay

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This <c>MonoBehavior</c> is responsible for raising <c>GameEvent</c>s when the 
/// player input changes.
/// </summary>
public class InputActionsReader : MonoBehaviour
{
    [Header("Gameplay Action Map")]
    public Vector2GameEvent gameplayMoveEvent;
    public Vector2GameEvent gameplayLookEvent;
    public VoidGameEvent gameplayInteractPressed;
    public VoidGameEvent gameplayInteractReleased;
    public BooleanGameEvent gameplayJumpPressed;
    
    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();

        // gameplay action map callbacks
        inputActions.gameplay.move.performed += OnMovePerformed;
        inputActions.gameplay.move.canceled += OnMoveCancelled;
        inputActions.gameplay.look.performed += OnLookPerformed;
        inputActions.gameplay.look.canceled += OnLookCancelled;
        inputActions.gameplay.interact.performed += OnInteractPressed;
        inputActions.gameplay.interact.canceled += OnInteractReleased;
        inputActions.gameplay.jump.performed += OnJumpPressed;
        inputActions.gameplay.jump.canceled += OnJumpReleased;
    }

    private void OnEnable()
    {
        inputActions.gameplay.Enable();
    }

    private void OnDisable()
    {
        inputActions.gameplay.Disable();
    }


    #region GAMEPLAY_ACTION_MAP
    /// <summary>
    /// Callback to perform when the <c>gameplay.move</c> action is performed - this 
    /// occurs when there is a new input value available.
    /// </summary>
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        gameplayMoveEvent.Raise(moveInput);
    }

    /// <summary>
    /// Callback to perform when the <c>gameplay.move</c> action is canceled - this 
    /// occurs when the player releases the stick to the neutral position.
    /// </summary>
    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        gameplayMoveEvent.Raise(Vector2.zero);
    }

    /// <summary>
    /// Callback to perform when the <c>gameplay.look</c> action is performed - this 
    /// occurs when there is a new input value available.
    /// </summary>
    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        gameplayLookEvent.Raise(moveInput);
    }

    /// <summary>
    /// Callback to perform when the <c>gameplay.look</c> action is canceled - this 
    /// occurs when the player releases the stick to the neutral position.
    /// </summary>
    private void OnLookCancelled(InputAction.CallbackContext context)
    {
        gameplayLookEvent.Raise(Vector2.zero);
    }

    /// <summary>Callback to perform when the <c>gameplay.action</c> action is pressed.</summary>
    private void OnInteractPressed(InputAction.CallbackContext _context) { gameplayInteractPressed.Raise(); }

    /// <summary>Callback to perform when the <c>gameplay.action</c> action is released.</summary>
    private void OnInteractReleased(InputAction.CallbackContext _context) { gameplayInteractReleased.Raise(); }

    private void OnJumpPressed(InputAction.CallbackContext _context) { gameplayJumpPressed.Raise(true); }
    private void OnJumpReleased(InputAction.CallbackContext _context) { gameplayJumpPressed.Raise(false); }
    #endregion
}
