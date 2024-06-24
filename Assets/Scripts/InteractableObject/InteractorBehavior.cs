// Ignore Spelling: Interactor

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorBehavior : MonoBehaviour
{
    private IInteractable selected;
    private Transform selectedTransform;
    private float selectedDistance = float.PositiveInfinity;

    [Tooltip("Physics Layer to trigger focusing on an object")]
    public PhysicsLayer_SO focusLayer;
    [Tooltip("Physics Layer for selecting and interacting with the closest object")]
    public PhysicsLayer_SO selectLayer;

    /// <summary>
    /// If the provided Collider object is closer than the currently active one, "select"
    /// this one and call the <c>.Select()</c> interface method on its parent <c>GameObject</c>.
    /// </summary>
    /// <param name="other">Collider to use for distance check</param>
    private void SelectIfCloser(Collider other)
    {
        float otherDistance = Vector3.Distance(transform.position, other.transform.position);
        if (otherDistance < selectedDistance)
        {
            selected?.Deselect();

            selected = other.GetComponentInParent<IInteractable>();
            selected.Select();
            selectedTransform = other.transform;
            selectedDistance = otherDistance;
        }
    }

    /// <summary>Update Focus/Selection when entering Collider.</summary>
    private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.layer == focusLayer.LayerIndex) { 
            other.gameObject.GetComponentInParent<IInteractable>()?.Focus(); 
        }

        else if (other.gameObject.layer == selectLayer.LayerIndex)
        {
            var iinteractable = other.gameObject.GetComponentInParent<IInteractable>();
            if (iinteractable != null) SelectIfCloser(other);
        }
    }

    /// <summary>Update Focus/Selection when staying within Collider.</summary>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == selectLayer.LayerIndex)
        {
            var iinteractable = other.gameObject.GetComponentInParent<IInteractable>();
            if (iinteractable != null) SelectIfCloser(other);
        }
    }

    /// <summary>Update Focus/Selection when exiting Collider.</summary
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == focusLayer.LayerIndex)
        {
            other.gameObject.GetComponentInParent<IInteractable>()?.Unfocus();
        }

        else if (other.gameObject.layer == selectLayer.LayerIndex)
        {
            var iinteractable = other.gameObject.GetComponentInParent<IInteractable>();
            if (iinteractable == selected)
            {
                selected.Deselect();

                selected = null;
                selectedTransform = null;
                selectedDistance = float.PositiveInfinity;
            }
        }
    }

    /// <summary>Updates <c>selectedDistance</c> on each physics update.</summary>
    private void FixedUpdate()
    {
        if (selectedTransform != null)
        {
            selectedDistance = Vector3.Distance(transform.position, selectedTransform.position);
        }
    }

    /// <summary>Called <c>.InteractPressed()</c> on the currently selected object.</summary>
    public void OnInteractPressed() { selected?.InteractPressed(); }

    public void OnInteractReleased() { selected?.InteractReleased(); }
}

