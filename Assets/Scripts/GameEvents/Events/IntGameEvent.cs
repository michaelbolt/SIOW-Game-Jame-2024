using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Game Event with a single <c>int</c> argument.</summary>
[CreateAssetMenu(fileName = "New Int Event", menuName = "Game Events/Int Event")]
public class IntGameEvent : BaseGameEvent<int> { }
