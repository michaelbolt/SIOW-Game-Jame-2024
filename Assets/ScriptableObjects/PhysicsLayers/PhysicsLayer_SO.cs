using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Asset to represent a single fixed Physics Layer.</summary>
[CreateAssetMenu(fileName ="New Physics Layer", menuName ="Physics Layer")]
public class PhysicsLayer_SO : ScriptableObject
{

    [Tooltip("Name of Layer to represent")]
    [SerializeField] private string layerName;
    public string LayerName { get { return layerName; } }

    [SerializeField] private int layerIndex;
    public int LayerIndex { get { return layerIndex; } }

    private string layerName_last;
    private int layerIndex_last;

    /// <summary>Update Layer Name / Index based on the changed value.</summary>
    private void OnValidate()
    {
        if (layerName != layerName_last)
        {
            layerName_last = layerName;

            layerIndex = LayerMask.NameToLayer(layerName);
            layerIndex_last = layerIndex;
            if (layerIndex == -1) Debug.LogError("invalid Layer name: " + layerName);
        }
        
        else if (layerIndex != layerIndex_last)
        {
            layerIndex_last = layerIndex;

            layerName = LayerMask.LayerToName(layerIndex);
            layerName_last = layerName;
            if (layerName == "") Debug.LogError("invalid Layer index: " + layerIndex);
        }
        
        
    }

}
