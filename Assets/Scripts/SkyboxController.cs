using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public bool fullyOpenWindow = false;
    public bool animating = false;


    private float cutoffValue;
    private float t = 0.0f;
    private float maxCutoff = 1;
    private float minCutoff = 0;
    private float changePerSecond;
    public float duration = 3;

    public Material Skybox;

    public static event Action WindowOpenEvent;

    public IEnumerator OpenSeethroughWindow()
    {
        if (!fullyOpenWindow && !animating)
        {
            changePerSecond = (maxCutoff - minCutoff) / duration;
            Material mats = Skybox;
            mats.SetFloat("_Cutoff", 0);
            while (mats.GetFloat("_Cutoff") < 1)
            {
                animating = true;
                mats.SetFloat("_Cutoff", Mathf.Clamp(mats.GetFloat("_Cutoff") + changePerSecond * Time.deltaTime, minCutoff, maxCutoff));
                t += Time.deltaTime;
                yield return null;
            }
            animating = false;
            fullyOpenWindow = true;
            Debug.Log("WINDOW OPEN EVENT INVOKE");
            WindowOpenEvent?.Invoke();
        }
        yield return null;
    }

    public IEnumerator CloseSeethroughWindow()
    {
        if (fullyOpenWindow && !animating)
        {
            changePerSecond = (minCutoff - maxCutoff) / duration;
            Material mats = Skybox;
            mats.SetFloat("_Cutoff", 1);
            while (mats.GetFloat("_Cutoff") > 0)
            {
                animating = true;
                mats.SetFloat("_Cutoff", Mathf.Clamp(mats.GetFloat("_Cutoff") + changePerSecond * Time.deltaTime, minCutoff, maxCutoff));
                t += Time.deltaTime;
                yield return null;
            }
            animating = false;
            fullyOpenWindow = false;
        }
        yield return null;
    }
}
