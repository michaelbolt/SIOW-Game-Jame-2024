// Ignore Spelling: Interactable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Required interface for a <c>GameObject</c> to be interacted with by an<c>Interactor</c>.
/// </summary>
public interface IInteractable
{

    /// <summary>
    /// Called when this object enters focus: <c>Interactor</c> is within a child 
    /// <c>Collider</c> on the <c>Interactor</c>'s Focus physics layer.
    /// </summary>
    public void Focus();

    /// <summary>
    /// Called when this object leaves focus: <c>Interactor</c> is no longer within a 
    /// child <c>Collider</c> on the <c>Interactor</c>'s Focus physics layer.
    /// </summary>
    public void Unfocus();

    /// <summary>
    /// Called when this object is selected: <c>Interactor</c> is within a child 
    /// <c>Collider</c> on the <c>Interactor</c>'s Select physics layer <em>and</em>is 
    /// the closest to the <c>Interactor</c>.
    /// </summary>
    public void Select();

    /// <summary>
    /// Called when this object is de-selected: <c>Interactor</c> leaves a child 
    /// <c>Collider</c> on the <c>Interactor</c>'s Select physics layer <em>or</em>is 
    /// no longer the closest to the <c>Interactor</c>.
    /// </summary>
    public void Deselect();

    /// <summary>
    /// Called when the <c>Interactor</c> pressed the interact button <em>and</em> this 
    /// object is selected.
    /// </summary>
    public void Interact();
}
