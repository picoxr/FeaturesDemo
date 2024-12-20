using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.XR.PXR.PXR_Input;

public class SeethroughManager : MonoBehaviour
{

    [HideInInspector]
    public bool seethroughActivated = false;

    public GameObject[] seethroughGameObjects;

    void OnApplicationPause(bool pause)
    {
        if (!pause && seethroughActivated)
        {
            PXR_Manager.EnableVideoSeeThrough = true;
        }
    }

    public void SwitchSeethrough()
    {
        if (seethroughActivated)
        {
            DeactivateSeethrough();
        }
        else
        {
            ActivateSeethrough();
        }
    }

    public void ActivateSeethrough()
    {
        seethroughActivated = true;
        PXR_Manager.EnableVideoSeeThrough = true;

        foreach (GameObject g in seethroughGameObjects)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }
    }

    public void DeactivateSeethrough()
    {
        seethroughActivated = false;
        PXR_Manager.EnableVideoSeeThrough = false;

        foreach (GameObject g in seethroughGameObjects)
        {
            if (g != null)
            {
                g.SetActive(true);
            }
        }
    }


}
