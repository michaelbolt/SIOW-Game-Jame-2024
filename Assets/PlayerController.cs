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
            if (moveMagnitude > 0)
            {
                // determine the player's intended direction of travel by rotating myCamera.transform.forward by the
                Vector3 myCameraForwardWorldSpace = Quaternion.AngleAxis(moveAngle, Vector3.up) * ProjectForwardOntoXZPlane(myCamera.transform);
                float rotationAngle = Vector3.SignedAngle(myCameraForwardWorldSpace, transform.forward, transform.up);
                float roationSign = -Mathf.Sign(rotationAngle);

                //rotate towards myCameraForwardWorldSpace by the 
                transform.Rotate(Vector3.up, roationSign * turnSpeed * Time.deltaTime);
            }

        }
    }

    #region Movement
    [Tooltip("Camera to base movement on")]
    public Camera myCamera;

    [Tooltip("Turning speed of character (deg / sec)")]
    public float turnSpeed = 90.0f;

    // input values
    private Vector2 moveInput = Vector2.zero;
    private float moveAngle = 0.0f;
    private float moveMagnitude = 0.0f;

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
