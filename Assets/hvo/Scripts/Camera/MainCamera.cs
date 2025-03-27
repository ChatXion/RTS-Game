using UnityEngine;

//got code from chatgpt
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f; // Speed of camera movement, adjustable in Inspector

    [SerializeField]
    private float zoomSpeed = 2f; // Speed of zooming, adjustable in Inspector

    [SerializeField]
    private float minZoom = 1f;   // Minimum zoom level (for orthographic size or FOV)

    [SerializeField]
    private float maxZoom = 10f;  // Maximum zoom level (for orthographic size or FOV)

    [SerializeField]
    private bool useBoundaries = false; // Toggle to enable/disable movement boundaries

    [SerializeField]
    private Vector2 minBounds = new Vector2(-10f, -10f); // Minimum X and Y position

    [SerializeField]
    private Vector2 maxBounds = new Vector2(10f, 10f);   // Maximum X and Y position

    private Camera cam; // Reference to the camera component

    void Start()
    {
        // Cache the camera component for efficiency
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraController must be attached to a GameObject with a Camera component!");
        }
    }

    void Update()
    {
        // Handle WASD movement
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float verticalInput = Input.GetAxisRaw("Vertical");     // W/S or Up/Down
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;

        if (useBoundaries)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = newPosition;

        // Handle zooming with middle mouse scroll wheel
        if (cam != null)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0f) // Only zoom if there's scroll input
            {
                if (cam.orthographic)
                {
                    // Orthographic camera: Adjust orthographicSize (smaller = zoom in, larger = zoom out)
                    cam.orthographicSize -= scrollInput * zoomSpeed;
                    cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
                }
                else
                {
                    // Perspective camera: Adjust field of view (smaller = zoom in, larger = zoom out)
                    cam.fieldOfView -= scrollInput * zoomSpeed * 10f; // Multiply for noticeable effect
                    cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
                }
            }
        }
    }
}