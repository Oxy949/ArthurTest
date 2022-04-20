using UnityEngine;

/// <summary>
/// Used to rotate the object with mouse
/// </summary>
[RequireComponent(typeof(Collider))] // Need a collider to handle OnMouseDown()
public class Rotator : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] private float rotationSpeed = 15;

    private bool _doRotate = false;

    private void OnMouseDown()
    {
        // Begin rotation in Update
        _doRotate = true;
    }

    private void Update()
    {
        // only after mouse down
        if (!_doRotate)
            return;

        var input = new Vector3()
        {
            x = Input.GetAxis("Mouse Y"),
            y = -Input.GetAxis("Mouse X"),
            z = 0,
        };

        var rotation = input * (360 * rotationSpeed * Time.deltaTime);
        
        // Rotate object
        transform.Rotate(rotation, Space.World);

        // Mouse up to stop the rotation
        if (Input.GetMouseButtonUp(0))
            _doRotate = false;
    }
}
