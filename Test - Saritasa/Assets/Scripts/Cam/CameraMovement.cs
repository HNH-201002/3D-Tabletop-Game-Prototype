using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10);
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private float rotationSpeed = 70.0f;
    [SerializeField] private float zoomSpeed = 4.0f;
    [SerializeField] private float minZoom = 5.0f;
    [SerializeField] private float maxZoom = 20.0f;
    [SerializeField] private float moveSpeed = 10.0f; 

    private Vector3 currentOffset;
    private Vector3 lastMousePosition;

    private void Start()
    {
        currentOffset = offset;
        lastMousePosition = Input.mousePosition; 
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("TopDownCameraController: Target not assigned.");
            return;
        }

        Vector3 desiredPosition = target.position + currentOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        HandleRotation();

        HandleZoom();

        HandleMovement();
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1)) 
        {
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float verticalRotation = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            transform.RotateAround(target.position, Vector3.up, horizontalRotation);
         
            transform.RotateAround(target.position, transform.right, verticalRotation);

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
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * moveSpeed * Time.deltaTime;
            move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * move; 
            target.Translate(move, Space.World);
        }

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
