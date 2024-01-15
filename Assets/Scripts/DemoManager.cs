using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.XR.PXR.PXR_Input;

public class DemoManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] IntroSequenceObjects;

    [SerializeField]
    private GameObject[] DemoObjects;

    [SerializeField]
    private GameObject introButton;

    [SerializeField]
    public ControlInputsManager controlInputsManager;

    public static DemoManager instance = null;

    /// <summary>
    /// Singleton for DemoManager
    /// </summary>
    /// <returns>The DemoManager instance</returns>
    public static DemoManager GetInstance()
    {
        if (!instance)
        {
            instance = FindObjectOfType<DemoManager>(); ;
        }
        return instance;
    }

    void Start()
    {
        ActivateEventSequence();
    }


    /// <summary>
    /// This method activates the intro sequence of the demo by starting the ActivateIntroSequence() coroutine.
    /// </summary>
    void ActivateEventSequence()
    {
        StartCoroutine(ActivateIntroSequence());
    }


    #region Time constants for intro sequence
    const float timeToAppear = 2f;
    const float timeToDisappear = 3f;
    const float timeToStartDemo = 5f;

    #endregion

    /// <summary>
    /// This coroutine method is responsible for activating the intro sequence of the demo.
    /// </summary>
    /// <remarks>
    /// It starts by setting the boolean value DemoInitiated to true in the ControlInputsManager script.
    /// It then loops through each object in the IntroSequenceObjects array, waits for 2 seconds, and fades the object into view
    /// by calling the SwitchVisibility() method of the TextAlphaSwitch component attached to the object. If the object also contains
    /// a ParticleSender component, it starts a coroutine to initialize the particles. After another 3 seconds, the object fades out of
    /// view by calling the SwitchVisibility() method again. This process repeats for each object in the array. After a final wait of
    /// 5 seconds, the method calls the ActivateDemoEnvironment() method to activate the demo environment.
    /// </remarks>
    /// 
    IEnumerator ActivateIntroSequence()
    {
        controlInputsManager.DemoInitiated = true; 

        foreach (GameObject obj in IntroSequenceObjects)
        {
            yield return new WaitForSeconds(timeToAppear);
            obj.GetComponent<TextAlphaSwitch>().SwitchVisibility();

            if (obj.GetComponentInChildren<ParticleSender>())
            {
                StartCoroutine(obj.GetComponentInChildren<ParticleSender>().InitParticles());
            }

            yield return new WaitForSeconds(timeToDisappear);
            obj.GetComponent<TextAlphaSwitch>().SwitchVisibility();
        }

        yield return new WaitForSeconds(timeToStartDemo);
        ActivateDemoEnvironment();
    }

    /// <summary>
    /// Skips the demo introduction
    /// </summary>
    public void SkipIntro()
    {
        StopAllCoroutines();
        
        ActivateDemoEnvironment();

    }

    /// <summary>
    /// Activates the Demo GameObjects.
    /// </summary>
    public void ActivateDemoEnvironment()
    {
        foreach(GameObject obj in IntroSequenceObjects)
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
        }

        if (introButton.activeInHierarchy) { 
            introButton.SetActive(false); 
        }

        foreach (GameObject obj in DemoObjects)
        {
            obj.SetActive(true);
        }
    }

}

