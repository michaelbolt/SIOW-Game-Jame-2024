using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When the inspector-linked <c>GameEvent</c> is raised, call the linked <c>UnityEvent</c>.
/// with <em>no</em> arguments.
/// </summary>
public class VoidGameEventListener : BaseGameEventListener<Void, VoidGameEvent, UnityVoidEvent> { }
