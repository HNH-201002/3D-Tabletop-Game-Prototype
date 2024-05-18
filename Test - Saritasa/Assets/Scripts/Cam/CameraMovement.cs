using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // The target the camera will follow
    public Vector3 offset = new Vector3(0, 10, -10); // Offset from the target
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement
    public float rotationSpeed = 70.0f; // Speed of camera rotation
    public float zoomSpeed = 4.0f; // Speed of zooming in and out
    public float minZoom = 5.0f; // Minimum zoom distance
    public float maxZoom = 20.0f; // Maximum zoom distance
    public float moveSpeed = 10.0f; // Speed of moving the camera

    private Vector3 currentOffset;
    private Vector3 lastMousePosition;

    private void Start()
    {
        currentOffset = offset;
        lastMousePosition = Input.mousePosition; // Initialize lastMousePosition to avoid jump
    }

    private void LateUpdate()
    {
        // Ensure the target is assigned
        if (target == null)
        {
            Debug.LogWarning("TopDownCameraController: Target not assigned.");
            return;
        }

        // Calculate the desired position
        Vector3 desiredPosition = target.position + currentOffset;
        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Handle camera rotation
        HandleRotation();

        // Handle camera zoom
        HandleZoom();

        // Handle camera movement
        HandleMovement();
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1)) // Rotate when right mouse button is held down
        {
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float verticalRotation = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Rotate the camera around the target on the y-axis (horizontal rotation)
            transform.RotateAround(target.position, Vector3.up, horizontalRotation);
            // Rotate the camera around the target on the x-axis (vertical rotation)
            transform.RotateAround(target.position, transform.right, verticalRotation);

            // Update the offset after rotation
            currentOffset = transform.position - target.position;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentOffset = Vector3.ClampMagnitude(currentOffset * (1 - scroll), maxZoom);
        if (currentOffset.magnitude < minZoom)
        {
            currentOffset = currentOffset.normalized * minZoom;
        }
    }

    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition; // Update lastMousePosition on mouse button down
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * moveSpeed * Time.deltaTime;
            move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * move; // Rotate move vector with camera's Y rotation
            target.Translate(move, Space.World);
        }

        // Handle movement with keyboard input
        Vector3 moveInput = new Vector3();
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveInput += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveInput += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveInput += Vector3.right;
        }
        target.Translate(moveInput * moveSpeed * Time.deltaTime, Space.World);
    }
}
