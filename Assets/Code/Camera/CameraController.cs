using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The target the camera will follow and look at.")]
    public Transform target;

    [Header("Rotation Settings")]
    [Tooltip("The speed at which the camera rotates around the target when using the mouse.")]
    public float rotationSpeed = 2f;

    [Tooltip("The speed at which the camera rotates around the target when using the keyboard.")]
    public float keyboardRotationSpeed = 50f;

    [Tooltip("The amount of zoom effect applied when the camera is rotating.")]
    public float rotationZoomFactor = 0.5f;

    [Tooltip("The speed at which the camera returns to its original zoom level after rotating.")]
    public float zoomReturnSpeed = 2f;

    [Header("Zoom Settings")]
    [Tooltip("The speed at which the camera zooms in and out when using the mouse scroll wheel.")]
    public float zoomSpeed = 2f;

    [Tooltip("The speed at which the camera zooms in and out when using the keyboard.")]
    public float keyboardZoomSpeed = 5f;

    [Tooltip("The minimum allowed distance between the camera and the target.")]
    public float minDistance = 2f;

    [Tooltip("The maximum allowed distance between the camera and the target.")]
    public float maxDistance = 20f;

    [Header("Smoothing Settings")]
    [Tooltip("The time it takes for the camera to smoothly transition to its target position.")]
    public float smoothTime = 0.1f;

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
