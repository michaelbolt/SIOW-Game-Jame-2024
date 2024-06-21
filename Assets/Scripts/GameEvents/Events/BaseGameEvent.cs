using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEvent<T> : ScriptableObject
{
    private readonly List<IGameEventListener<T>> eventListeners = new();

    /// <summary>
    /// Raise this <c>GameEvent</c> and call <c>.OnEventRaised(item)</c> on all 
    /// registered listeners.
    /// </summary>
    /// <param name="item">
    /// Item to provide to <c>.OnEventRaised()</c> for each  registered listener.
    /// </param>
    public void Raise(T item)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(item);
        }
    }

    /// <summary>
    /// Register the provided listener to be notified whenever this <c>GameEvent</c> is raised
    /// and call the listener's <c>OnEventRaised()</c> method.
    /// </summary>
    public void RegisterListener(IGameEventListener<T> listener)
    {
        if (!eventListeners.Contains(listener)) { eventListeners.Add(listener); }
    }

    /// <summary>
    /// Unregister the provided listener so that it will not be notified whenever this
    /// <c>GameEvent</c> is raised.
    /// </summary>
    public void UnregisterListener(IGameEventListener<T> listener)
    {
       if (eventListeners.Contains(listener)) { eventListeners.Remove(listener);  }
    }
}
