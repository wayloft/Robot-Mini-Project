using UnityEngine;
using UnityEngine.Events;

public class AttachablePart : MonoBehaviour
{
    public Transform defaultAttachPoint; // The point where this part should reattach
    public AttachablePart rootPart; // Reference to the root part of the robot

    public UnityEvent OnAttach;
    public UnityEvent OnDetach;

    private bool isDetached = false;

    [SerializeField] private bool isMovable = true; // Allow the part to be grabbed initially

    private Quaternion originalLocalRotation;
    private Vector3 originalLocalPosition;

    void Start()
    {
        originalLocalRotation = transform.localRotation;
        originalLocalPosition = transform.localPosition;

        // If no rootPart is set, assume this is the root part
        if (rootPart == null)
        {
            rootPart = this;
        }
    }

    public void Detach()
    {
        transform.SetParent(null);
        isDetached = true;
        isMovable = true;
        OnDetach.Invoke();
        Debug.Log("Detached");
    }

    public void Attach()
    {
        if (defaultAttachPoint != null)
        {
            rootPart.transform.SetParent(defaultAttachPoint);
            rootPart.transform.localPosition = originalLocalPosition;
            rootPart.transform.localRotation = originalLocalRotation;
        }
        isDetached = false;
        isMovable = true;

        OnAttach.Invoke();
    }

    public bool IsDetached()
    {
        return isDetached;
    }

    public bool IsMovable()
    {
        return isMovable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rootPart == this && isDetached && other.transform == defaultAttachPoint)
        {
            Debug.Log("Attempted to attach");
            Attach();
        }
    }
}
