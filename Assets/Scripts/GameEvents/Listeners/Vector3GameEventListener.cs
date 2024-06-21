using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When the inspector-linked <c>GameEvent</c> is raised, call the linked <c>UnityEvent</c>
/// with a single <c>Vector3</c> argument.
/// </summary>
public class Vector3GameEventListener : BaseGameEventListener<Vector3, Vector3GameEvent, UnityVector3Event> { }
