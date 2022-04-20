using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Gizmo point to use with <see cref="ResizeGizmo"/>
/// </summary>
[RequireComponent(typeof(Collider))]
public class ResizeAnchorPoint : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private ResizeGizmo resizeGizmo;

    [Header("Options:")] 
    [SerializeField] private Vector3 resizeAxis = Vector3.right;
    [SerializeField] private Vector3 movePlaneNormal = Vector3.up;

    // Is point dragging now?
    private bool _isDragging;


    // Save original mouse offset
    private Vector3 _originalHitPoint;
    private Vector3 _offset;


    // cache some objs
    private Camera _targetCamera;

    /// <summary>
    /// Sets point position from target
    /// </summary>
    /// <param name="fromScale">Target scale</param>
    /// <param name="reset">Reset point origin to 0</param>
    public void UpdatePosition(Vector3 fromScale, bool reset = false)
    {
        if (reset)
            transform.localPosition = Vector3.zero;

        // Calculate position from scale
        var localPosition = transform.localPosition;
        localPosition = new Vector3(
            Mathf.Lerp(localPosition.x, fromScale.x * resizeAxis.x, Mathf.Abs(resizeAxis.x)),
            Mathf.Lerp(localPosition.y, fromScale.y * resizeAxis.y, Mathf.Abs(resizeAxis.y)),
            Mathf.Lerp(localPosition.z, fromScale.z * resizeAxis.z, Mathf.Abs(resizeAxis.z)));
        transform.localPosition = localPosition;
    }

    /// <summary>
    /// Sets point position from target
    /// </summary>
    /// <param name="fromTransform">target transform</param>
    /// <param name="reset"></param>
    public void UpdatePosition(Transform fromTransform, bool reset = false)
    {
        var targetScale = fromTransform.localScale;
        UpdatePosition(targetScale / 2.0f, reset); // /2 because of collider
    }

    private void Start()
    {
        _targetCamera = Camera.main; // TODO: Replace if multiple cameras
    }

    private void OnMouseDown()
    {
        if (!resizeGizmo)
            return;

        _originalHitPoint = resizeGizmo.transform.position;

        // Find mouse position in world
        Ray ray = _targetCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            _originalHitPoint = hit.point;
            _offset = transform.position - _originalHitPoint;
        }

        // begin dragging in Update
        _isDragging = true;
    }

    private void Update()
    {
        if (!resizeGizmo)
            return;

        if (_isDragging)
        {
            // Find cursor position
            Vector3 curPosition = GetDraggingPoint() + _offset;

            // Calc local position
            Vector3 localPos = resizeGizmo.transform.InverseTransformPoint(curPosition);

            // Update point position
            UpdatePosition(localPos);

            // Calc localScale
            var localPosition = transform.localPosition;
            Vector3 fixedScale = new Vector3(
                localPosition.x * resizeAxis.x,
                localPosition.y * resizeAxis.y,
                localPosition.z * resizeAxis.z) * 2; // *2 because of collider

            // Send it to ResizeGizmo
            resizeGizmo.SetScale(resizeAxis, fixedScale);

            // Stops when mouse up
            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }
        }
    }

    /// <summary>
    /// Calculates cursor position
    /// </summary>
    /// <returns>cursor position</returns>
    private Vector3 GetDraggingPoint()
    {
        Ray ray = _targetCamera.ScreenPointToRay(Input.mousePosition);
        Plane xzPlane = new Plane(resizeGizmo.transform.TransformDirection(movePlaneNormal), _originalHitPoint);
        xzPlane.Raycast(ray, out var distance);
        var cursorWorldPosition = ray.GetPoint(distance);
        return cursorWorldPosition;
    }
}