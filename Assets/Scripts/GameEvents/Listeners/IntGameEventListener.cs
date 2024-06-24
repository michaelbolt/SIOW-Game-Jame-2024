using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When the inspector-linked <c>GameEvent</c> is raised, call the linked <c>UnityEvent</c>
/// with a single <c>int</c> argument.
/// </summary>
public class IntGameEventListener : BaseGameEventListener<int, IntGameEvent, UnityIntEvent> { }
