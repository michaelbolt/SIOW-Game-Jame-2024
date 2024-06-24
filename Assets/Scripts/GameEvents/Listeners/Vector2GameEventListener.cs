using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When the inspector-linked <c>GameEvent</c> is raised, call the linked <c>UnityEvent</c>
/// with a single <c>Vector2</c> argument.
/// </summary>
public class Vector2GameEventListener : BaseGameEventListener<Vector2, Vector2GameEvent, UnityVector2Event> { }
