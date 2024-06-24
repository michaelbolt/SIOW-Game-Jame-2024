using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// User-defined <c>void</c> <c>strict</c> to serve a stand-in within the Event system.
/// </summary>
/// <remarks>
/// The event system requires an explicit typed argument to be passed; this <c>Void</c>
/// type allows events to be registered with no arguments.
/// </remarks>
[System.Serializable] public struct Void { }
