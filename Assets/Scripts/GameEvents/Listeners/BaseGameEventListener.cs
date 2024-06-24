using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// When the inspector-linked <c>GameEvent</c> is raised, call the linked <c>UnityEvent</c>.
/// </summary>
public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, 
    IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
{

    /// <summary><c>BaseGameEvent</c> child type that we are listening for.</summary>
    [Tooltip("Game Event to listen for")]
    [SerializeField] private E gameEvent;
    public E GameEvent {  get { return gameEvent; } set { gameEvent = value; } }


    [Tooltip("Unity Event to call when the linked Game Event is raised")]
    [SerializeField] private UER unityEventResponse;

    /// <summary>
    /// Register this listener to the target <c>GameEvent</c> (if set) when enabled.
    /// </summary>
    private void OnEnable()
    {
        if (gameEvent == null) { return; }
        GameEvent.RegisterListener(this);
    }

    /// <summary>
    /// Unregister this listener from the target <c>GameEvent</c> (if set) when disabled.
    /// </summary>
    private void OnDisable()
    {
        if (gameEvent == null) { return; }
        GameEvent.UnregisterListener(this);
    }

    /// <summary>
    /// Invoke the linked <c>UnityEvent</c> when the linked <c>GameEvent</c> is raised.
    /// </summary>
    /// <param name="item"></param>
    public void OnEventRaised(T item)
    {
        if (unityEventResponse == null) { return; }
        unityEventResponse.Invoke(item);
    }
}
