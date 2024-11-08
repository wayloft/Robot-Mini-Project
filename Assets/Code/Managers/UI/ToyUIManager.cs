using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToyViewer
{
    public class ToyUIManager : MonoBehaviour
    {
        [SerializeField] Text statusText;

        private void OnEnable()
        {
            if (AttachedPartManager.Instance != null)
                AttachedPartManager.Instance.PartStatusUpdate += UpdateStatusText;
        }

        private void OnDisable()
        {
            if (AttachedPartManager.Instance != null)
                AttachedPartManager.Instance.PartStatusUpdate -= UpdateStatusText;
        }

        public void UpdateStatusText()
        {
            if (AttachedPartManager.Instance.AreAnyPartsDetached())
            {
                UpdateStatusDetach();
            }
            else
            {
                UpdateStatusAttach();
            }
        }

        private void UpdateStatusDetach()
        {
            statusText.text = "Detached";
            statusText.color = Color.red;
        }

        private void UpdateStatusAttach()
        {
            statusText.text = "Attached";
            statusText.color = Color.green;
        }
    }

}
