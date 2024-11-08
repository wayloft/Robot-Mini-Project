using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedPartManager : Singleton<AttachedPartManager>
{
    List<AttachablePart> attachablePartList = new List<AttachablePart>();

    public delegate void PartStatus();
    public event PartStatus PartStatusUpdate;

    public void RegisterPart(AttachablePart part)
    {
        if(!attachablePartList.Contains(part))
        {
            attachablePartList.Add(part);
            part.OnPartAttach += PartAttachUpdate;
            part.OnPartDetach += PartDetachUpdate;
        }
    }

    public void UnRegisterPart(AttachablePart part)
    {
        if (attachablePartList.Contains(part))
        {
            attachablePartList.Remove(part);
            part.OnPartAttach -= PartAttachUpdate;
            part.OnPartDetach -= PartDetachUpdate;
        }
    }

    private void PartAttachUpdate(AttachablePart part)
    {
        PartStatusUpdate?.Invoke();
    }

    private void PartDetachUpdate(AttachablePart part)
    {
        PartStatusUpdate?.Invoke();
    }

    public bool AreAnyPartsDetached()
    {
        foreach(AttachablePart part in attachablePartList)
        {
            if (part.IsDetached())
                return true;
        }

        return false;
    }
}
