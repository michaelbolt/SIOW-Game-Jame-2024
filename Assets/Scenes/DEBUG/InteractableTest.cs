using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : MonoBehaviour, IInteractable
{

    public void Focus() { gameObject.GetComponent<Renderer>().material.color = Color.cyan; }

    public void Unfocus() { gameObject.GetComponent<Renderer>().material.color = Color.white; }

    public void Select() { gameObject.GetComponent<Renderer>().material.color = Color.red; }

    public void Deselect() { gameObject.GetComponent<Renderer>().material.color = Color.cyan; }

    public void Interact() { Debug.Log("INTERACT!"); }
}
