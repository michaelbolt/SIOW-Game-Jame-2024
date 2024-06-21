using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Game Event with <em>no</em> arguments.</summary>
[CreateAssetMenu(fileName ="New Void Event", menuName ="Game Events/Void Event")]
public class VoidGameEvent : BaseGameEvent<Void>
{
    /// <summary>Raise the <c>GameEvent</c> with <em>no</em> arguments.</summary>
    public void Raise() { Raise(new Void()); }
}
