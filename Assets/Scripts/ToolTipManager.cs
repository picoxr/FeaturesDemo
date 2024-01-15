using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    public List<GameObject> TooltipCollection;
    public GameObject TriggerToolTip;
    public GameObject GripToolTip;

    private void Start()
    {
        TooltipCollection = new List<GameObject>
        {
            TriggerToolTip,
            GripToolTip
        };
    }

    public void EnableAllTooltips()
    {
        foreach (GameObject tooltip in TooltipCollection)
        {
            if (!tooltip.activeInHierarchy)
            {
                tooltip.SetActive(true);
            }
            
        }
    }

    public void DisableAllTooltips()
    {
        foreach (GameObject tooltip in TooltipCollection)
        {
            if (tooltip.activeInHierarchy)
            {
                tooltip.SetActive(false);
            }
        }
    }

    public void EnableTooltip(GameObject tooltip)
    {
        if (!tooltip.activeInHierarchy)
        {
            tooltip.SetActive(true);
        }
    }

    public void DisableTooltip(GameObject tooltip)
    {
        if (tooltip.activeInHierarchy)
        {
            tooltip.SetActive(false);
        }
    }

    public void SwitchTooltip(GameObject tooltip)
    {
        if (tooltip.activeInHierarchy)
        {
            tooltip.SetActive(false);
        }
        else
        {
            tooltip.SetActive(true);
        }
    }
}
