using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.XR.PXR.PXR_Input;

public class ControlInputsManager : MonoBehaviour
{
    public static event Action ControllersActive;
    public static event Action HandsActive;

    [SerializeField]
    private GameObject[] HandPrefabs;

    [SerializeField]
    public GameObject RightController;
    [SerializeField]
    public GameObject LeftController;

    public Material[] ControllerMaterials;

    [SerializeField]
    private GameObject handTrackingPanel;

    public bool DemoInitiated = false;

    [HideInInspector]
    public enum ControllerSide
    {
        RIGHT,
        LEFT
    }

    private bool controllersActive = false;

    void Start()
    {
        SeethroughWindowManager.TestedSeethroughEvent += HandTrackingReady;
    }

    /// <summary>
    /// This method is called every frame during runtime. If the demo has been initiated, it checks if hand tracking is active.
    /// If hand tracking is active, it checks if controllers are active, and activates hands if they are.
    /// If hand tracking is not active, it checks if controllers are inactive, and activates controllers if they are.
    /// </summary>
    void Update()
    {
        if (DemoInitiated)
        {
            if (PXR_HandTracking.GetActiveInputDevice() == ActiveInputDevice.HandTrackingActive)
            {
                if (controllersActive)
                {
                    ActivateHands();
                }
            }
            else
            {
                if (!controllersActive)
                {
                    ActivateControllers();
                }
            }
        }
    }

    #region Controllers

    /// <summary>
    /// Activates the controllers and invokes the ControllersActive event. Sets the variable controllersActive to true.
    /// Deactivates all hand prefabs.
    /// </summary>
    public void ActivateControllers()
    {
        controllersActive = true;

        ControllersActive?.Invoke();
        foreach (GameObject hand in HandPrefabs)
        {
            hand.SetActive(false);
        }
    }

    #region DISSOLVE PARAMS

    private float t = 0.0f;
    private float maxCutoff = 1;
    private float minCutoff = 0;
    private float changePerSecond;
    public float duration = .5f;

    #endregion
    /// <summary>
    /// IEnumerator to undissolve a model by changing the alpha cutoff value of its material over a certain duration
    /// </summary>
    /// <param name="model">The transform of the model to be undissolved</param>
    [Obsolete ("Not used")]
    public IEnumerator Undissolve(Transform model)
    {
        
        Material mats = model.GetComponentInChildren<SkinnedMeshRenderer>().material;
        changePerSecond = (minCutoff - maxCutoff) / duration;
        mats.SetFloat("_Cutoff", 1);
        while (mats.GetFloat("_Cutoff") > 0)
        {
            mats.SetFloat("_Cutoff", Mathf.Clamp(mats.GetFloat("_Cutoff") + changePerSecond * Time.deltaTime, minCutoff, maxCutoff));
            t += Time.deltaTime;

            yield return null;
        }
        yield return null;
    }

    /// <summary>
    /// Coroutine that gradually dissolves a skinned mesh object by modifying its material's _Cutoff parameter.
    /// </summary>
    /// <param name="model">The transform of the skinned mesh object to dissolve.</param>
    [Obsolete("Not used")]
    public IEnumerator Dissolve(Transform model)
    { 
        Material mats = model.GetComponentInChildren<SkinnedMeshRenderer>().material;
        changePerSecond = (maxCutoff - minCutoff) / duration;
        mats.SetFloat("_Cutoff", 0);
        while (mats.GetFloat("_Cutoff") < 1)
        {
            mats.SetFloat("_Cutoff", Mathf.Clamp(mats.GetFloat("_Cutoff") + changePerSecond * Time.deltaTime, minCutoff, maxCutoff));

            yield return null;
        }
        yield return null;
    }

    ///<summary>
    ///Enables the ray interaction for a specific controller.
    ///</summary>
    ///<param name="side">The side of the controller to enable ray interaction.</param>
    public void EnableControllerRayInteraction(ControllerSide side)
    {
        if (side == ControllerSide.RIGHT)
        {
            RightController.GetComponentInChildren<XRInteractorLineVisual>().enabled = true;
            RightController.GetComponentInChildren<LineRenderer>().enabled = true;
        }
        else
        {
            LeftController.GetComponentInChildren<XRInteractorLineVisual>().enabled = true;
            LeftController.GetComponentInChildren<LineRenderer>().enabled = true;
        }
    }

    /// <summary>
    /// Disables the ray interaction for the specified controller side.
    /// </summary>
    /// <param name="side">The controller side (left or right).</param>
    public void DisableControllerRayInteraction(ControllerSide side)
    {
        if (side == ControllerSide.RIGHT)
        {
            RightController.GetComponentInChildren<XRInteractorLineVisual>().enabled = false;
            RightController.GetComponentInChildren<LineRenderer>().enabled = false;
        }
        else
        {
            LeftController.GetComponentInChildren<XRInteractorLineVisual>().enabled = false;
            LeftController.GetComponentInChildren<LineRenderer>().enabled = false;
        }
    }

    #endregion

    #region Hand tracking

    public bool firstTimeHands = true;

    /// <summary>
    /// Method called when hand tracking is ready, enables hand tracking coroutine if it's the first time
    /// </summary>
    [Obsolete ("Not used")]
    void HandTrackingReady()
    {     
        if (firstTimeHands)
        {
            StartCoroutine(EnableHandTrackingCoroutine());
            firstTimeHands = false;
        } 
    }

    /// <summary>
    /// Coroutine to enable hand tracking after a delay of 1.5 seconds.
    /// Activates a UI panel and switches its visibility using a TextAlphaSwitch script.
    /// </summary>
    [Obsolete ("Not used")]
    public IEnumerator EnableHandTrackingCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        handTrackingPanel.SetActive(true);
        handTrackingPanel.GetComponentInChildren<TextAlphaSwitch>().SwitchVisibility();
    }

    /// <summary>
    /// Activates hand models and sets controllersActive flag to false.
    /// Invokes HandsActive event and sets HandPrefabs active.
    /// </summary>
    public void ActivateHands()
    {
        controllersActive = false;
        HandsActive?.Invoke();
        foreach (GameObject hand in HandPrefabs)
        {
            hand.SetActive(true);
        }
    }

    #endregion

    
}
