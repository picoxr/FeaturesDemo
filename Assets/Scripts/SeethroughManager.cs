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

    void OnApplicationPause(bool pause)
    {
        if (!pause && seethroughActivated)
        {
            PXR_Boundary.EnableSeeThroughManual(true);
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
        PXR_Boundary.EnableSeeThroughManual(true);
    }

    public void DeactivateSeethrough()
    {
        seethroughActivated = false;
        PXR_Boundary.EnableSeeThroughManual(false);
    }


}
