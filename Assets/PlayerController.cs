using System.Collections;
using System.Collections.Generic;
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

        // bind debug messages to Actions
        inputActions.gameplay.move.performed += context =>
        {
            Debug.Log("move: " + context.ReadValue<Vector2>());
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


    // Update is called once per frame
    void Update()
    {
        
    }

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
