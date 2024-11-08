using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToyViewer
{
    public class ToyInteractionManager : MonoBehaviour
    {
        public Transform toyTorso;

        [SerializeField]
        private List<AttachablePart> attachableParts = new List<AttachablePart>();

        public UnityEvent OnDrag;
        public UnityEvent OnRelease;

        private bool isDraggingPart = false;
        private Vector3 offset;
        private Vector3 initialDragPosition;

        private OutlineHandler outlinedPart = null;
        private AttachablePart currentPart = null;

        private Plane movementPlane;

        void Update()
        {
            HandleDrag();
            HandleMouseOver();
        }

        private void HandleDrag()
        {
            if (Input.GetMouseButtonDown(0) && !isDraggingPart)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    AttachablePart part = hit.transform.GetComponent<AttachablePart>();

                    // Check if part is valid and fetch the root part if available
                    if (part != null && part.rootPart != null && part.rootPart.IsDetachable())
                    {
                        currentPart = part.rootPart;
                        currentPart.Detach();
                        isDraggingPart = true;
                        initialDragPosition = currentPart.transform.position;
                        offset = currentPart.transform.position - hit.point;

                        OnDrag?.Invoke();

                        movementPlane = new Plane(Camera.main.transform.forward, currentPart.transform.position);

                        DisableOutlineAll();
                    }
                }
            }

            if (Input.GetMouseButton(0) && isDraggingPart && currentPart != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (!currentPart.IsDetached() && !currentPart.IsRootTorso)
                    return;


                if (movementPlane.Raycast(ray, out float enter))
                {
                    Vector3 targetPosition = ray.GetPoint(enter) + offset;

                    Vector3 cameraRight = Camera.main.transform.right;
                    Vector3 cameraUp = Camera.main.transform.up;

                    Vector3 relativePosition = Vector3.zero;
                    relativePosition += Vector3.Project(targetPosition - currentPart.transform.position, cameraRight);
                    relativePosition += Vector3.Project(targetPosition - currentPart.transform.position, cameraUp);

                    currentPart.transform.position += Vector3.Lerp(Vector3.zero, relativePosition, 5f * Time.deltaTime);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDraggingPart = false;
                currentPart = null;
                OnRelease?.Invoke();
            }
        }

        private void HandleMouseOver()
        {
            if (!isDraggingPart)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    AttachablePart part = hit.transform.GetComponent<AttachablePart>();

                    Debug.Log($"Found AttachablePart: {part}");

                    // Access root part and check if it is movable for outlining
                    if (part != null && part.rootPart != null && part.rootPart.IsDetachable())
                    {
                        Debug.Log($"Rootpart valid: {part}");
                        OutlineHandler newOutline = part.rootPart.GetComponent<OutlineHandler>();

                        // Only update outline if the part is different
                        if (outlinedPart != newOutline)
                        {
                            DisableOutlineAll();
                            outlinedPart = newOutline;
                            outlinedPart?.EnableOutline();
                        }
                        return;
                    }
                }
            }

            // If the raycast hits nothing or a non-movable part, disable outline
            DisableOutlineAll();
        }

        private void DisableOutlineAll()
        {
            if (outlinedPart != null)
            {
                outlinedPart.DisableOutline();
                outlinedPart = null;
            }
        }
    }

}
