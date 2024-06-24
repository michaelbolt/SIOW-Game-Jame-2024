using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Game Event with a single <c>bool</c> argument.</summary>
[CreateAssetMenu(fileName = "New Boolean Event", menuName = "Game Events/Boolean Event")]
public class BooleanGameEvent : BaseGameEvent<bool> { }
