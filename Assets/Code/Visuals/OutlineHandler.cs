using System.Collections.Generic;
using UnityEngine;

public class OutlineHandler : MonoBehaviour
{
    [SerializeField]
    private List<Outline> outlineComponents = new List<Outline>();

    private bool isOutlined = false;

    public void EnableOutline()
    {
        if (isOutlined) return;

        foreach (var outline in outlineComponents)
        {
            outline.enabled = true;
        }

        isOutlined = true;
    }

    public void DisableOutline()
    {
        if (!isOutlined) return;

        foreach (var outline in outlineComponents)
        {
            outline.enabled = false;
        }

        isOutlined = false;
    }

    public void RemoveFromList(Outline outline)
    {
        if(outlineComponents.Contains(outline))
        {
            outlineComponents.Remove(outline);
        }
    }

    public void AddToList(Outline outline)
    {
        if (!outlineComponents.Contains(outline))
        {
            outlineComponents.Add(outline);
        }
    }
}
