using UnityEngine;

/// <summary>
/// Resize gizmo. Uses <see cref="ResizeAnchorPoint"/> to resize the <see cref="target"/> object
/// </summary>
public class ResizeGizmo : MonoBehaviour
{
    [Header("Resize:")] 
    [SerializeField] private Transform target;

    private ResizeAnchorPoint[] _resizeAnchorPoints;

    /// <summary>
    /// Setup the gizmo
    /// </summary>
    /// <param name="newTarget">transform</param>
    public void AttachTo(Transform newTarget)
    {
        target = newTarget;
        UpdatePointsPosition();
    }

    /// <summary>
    /// Apples the scale to <see cref="target"/>
    /// </summary>
    /// <param name="axis">axis direction</param>
    /// <param name="values">axis values</param>
    public void SetScale(Vector3 axis, Vector3 values)
    {
        var targetScale = target.localScale;
        targetScale = new Vector3(
            Mathf.Lerp(targetScale.x, values.x, Mathf.Abs(axis.x)),
            Mathf.Lerp(targetScale.y, values.y, Mathf.Abs(axis.y)),
            Mathf.Lerp(targetScale.z, values.z, Mathf.Abs(axis.z)));
        target.localScale = targetScale;
        UpdatePointsPosition();
    }

    private void Start()
    {
        _resizeAnchorPoints = GetComponentsInChildren<ResizeAnchorPoint>();

        if (target)
            AttachTo(target);
    }

    private void Update()
    {
        if (!target)
            return;

        var transformCache = transform;
        transformCache.rotation = target.rotation;
        transformCache.position = target.position;
    }

    private void UpdatePointsPosition(bool resetPoints = false)
    {
        foreach (var point in _resizeAnchorPoints)
        {
            point.UpdatePosition(target, resetPoints);
        }
    }
}