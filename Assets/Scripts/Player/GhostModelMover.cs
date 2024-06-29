using UnityEngine;

/// <summary>This Component causes the model to bob up and down (like a ghost!).</summary>
public class GhostModelMover : MonoBehaviour
{
    [Tooltip("Number of bobs to make in one second")]
    public float bobFrequency = 0.7f;
    [Tooltip("Maximum vertical height of bobbing")]
    public float bobHeight = 0.1f;
    [Tooltip("Vertical offset to apply to bobbing")]
    public float bobOffset = 0.0f;

    private float modelOffset = 0.0f;

    // Update is called once per frame
    void Update()
    {
        float oldModelOffset = modelOffset;
        modelOffset = Mathf.Cos(2.0f * Mathf.PI * bobFrequency * Time.time) * bobHeight;
        transform.position = new Vector3(transform.position.x, transform.position.y - oldModelOffset + modelOffset, transform.position.z);
    }
}
