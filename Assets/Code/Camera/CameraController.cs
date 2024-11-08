using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 2f;
    public float keyboardRotationSpeed = 50f;
    public float zoomSpeed = 2f;
    public float keyboardZoomSpeed = 5f;
    public float minDistance = 2f;
    public float maxDistance = 20f;
    public float smoothTime = 0.1f;
    public float rotationZoomFactor = 0.5f;
    public float zoomReturnSpeed = 2f;

    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;
    private float rotationZoom = 0f;
    private bool cameraLocked = false;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        if (target == null) return;

        HandleRotation();
        HandleKeyboardRotation();
        HandleZoom();
        HandleKeyboardZoom();

        Vector3 targetPosition = target.position + offset.normalized * Mathf.Clamp(offset.magnitude + rotationZoom, minDistance, maxDistance);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        transform.LookAt(target);
    }

    public void LockCameraMovement(bool value)
    {
        cameraLocked = value;
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1) && !cameraLocked)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            Quaternion rotation = Quaternion.Euler(0, mouseX, 0);
            offset = rotation * offset;
        }
    }

    void HandleKeyboardRotation()
    {
        float horizontalRotation = 0f;
        bool isRotating = false;

        if(!cameraLocked)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalRotation = keyboardRotationSpeed * Time.deltaTime;
                isRotating = true;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                horizontalRotation = -keyboardRotationSpeed * Time.deltaTime;
                isRotating = true;
            }
        }


        Quaternion rotation = Quaternion.Euler(0, horizontalRotation, 0);
        offset = rotation * offset;



        if (isRotating)
        {
            rotationZoom = Mathf.Lerp(rotationZoom, -rotationZoomFactor, Time.deltaTime * zoomReturnSpeed);
        }
        else
        {
            rotationZoom = Mathf.Lerp(rotationZoom, 0f, Time.deltaTime * zoomReturnSpeed);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        offset = offset.normalized * Mathf.Clamp(offset.magnitude - scroll * zoomSpeed, minDistance, maxDistance);
    }

    void HandleKeyboardZoom()
    {
        float zoomInput = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) zoomInput = -keyboardZoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) zoomInput = keyboardZoomSpeed * Time.deltaTime;

        offset = offset.normalized * Mathf.Clamp(offset.magnitude + zoomInput, minDistance, maxDistance);
    }
}
