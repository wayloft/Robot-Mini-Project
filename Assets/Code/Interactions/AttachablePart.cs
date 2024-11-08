using UnityEngine;
using UnityEngine.Events;

public class AttachablePart : MonoBehaviour
{
    public Transform defaultAttachPoint; // The point where this part should reattach
    public AttachablePart rootPart; // Reference to the root part of the toy

    [SerializeField] 
    private bool isDetachable = false; // Allow the part to be grabbed initially

    public UnityEvent OnAttach;
    public UnityEvent OnDetach;

    private bool isDetached = false;
    private Transform originalParent;

    private bool isMovable = true; 

    private Quaternion originalLocalRotation;
    private Vector3 originalLocalPosition;

    void Start()
    {
        originalLocalRotation = transform.localRotation;
        originalLocalPosition = transform.localPosition;

        originalParent = transform.parent;

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
    }

    public void Attach()
    {
        if (defaultAttachPoint != null)
        {
            rootPart.transform.SetParent(originalParent);
            rootPart.transform.localPosition = originalLocalPosition;
            rootPart.transform.localRotation = originalLocalRotation;
        }
        isDetached = false;
        isMovable = false;


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

    public bool IsDetachable()
    {
        return isDetachable;
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
