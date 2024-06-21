using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When the inspector-linked <c>GameEvent</c> is raised, call the linked <c>UnityEvent</c>
/// with a single <c>float</c> argument.
/// </summary>
public class FloatGameEventListener : BaseGameEventListener<float, FloatGameEvent, UnityFloatEvent> { }
