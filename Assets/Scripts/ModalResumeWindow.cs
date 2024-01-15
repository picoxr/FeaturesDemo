using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModalResumeWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject ModalWindow;

    public void Start()
    {
        PXR_Plugin.System.FocusStateLost += FocusLost;
        PXR_Plugin.System.FocusStateAcquired += FocusAcquired;

        PXR_Enterprise.InitEnterpriseService();
        PXR_Enterprise.BindEnterpriseService();
    }

  
    private void FocusLost() {
        DemoManager.GetInstance().controlInputsManager.RightController.SetActive(false);
        DemoManager.GetInstance().controlInputsManager.LeftController.SetActive(false);
    }

    //bool sixdof = true;

    private void FocusAcquired()
    {
        
        DemoManager.GetInstance().controlInputsManager.RightController.SetActive(true);
        DemoManager.GetInstance().controlInputsManager.LeftController.SetActive(true);
        
        //DemoManager.GetInstance().controlInputsManager.EnableControllerRayInteraction(ControlInputsManager.ControllerSide.RIGHT);
        
        //StartCoroutine(DemoManager.GetInstance().controlInputsManager.EnableController(ControlInputsManager.ControllerSide.LEFT));
        //StartCoroutine(DemoManager.GetInstance().controlInputsManager.EnableController(ControlInputsManager.ControllerSide.RIGHT));

        ModalWindow.SetActive(true);
        /**
         *
        //if (sixdof){
        //    PXR_System.SwitchSystemFunction(SystemFunctionSwitchEnum.SFS_SIX_DOF_SWITCH, SwitchEnum.S_OFF);
        //}
        //else
        //{
        //    PXR_System.SwitchSystemFunction(SystemFunctionSwitchEnum.SFS_SIX_DOF_SWITCH, SwitchEnum.S_ON);
        //    PXR_System.SwitchSystemFunction(SystemFunctionSwitchEnum.SFS_SECURITY_ZONE_PERMANENTLY, SwitchEnum.S_ON);
        //}
         * 
         * 
         */



    }

    public void LaunchIPDCalibration()
    {

        PXR_Enterprise.StartActivity("com.picovr.provision", "com.picovr.provision2.DailyIpdActivity", "", "", new string[] { "CATEGORY_DEFAULT" }, new int[] {0});

    }

    public void Close()
    { 
        //DemoManager.GetInstance().controlInputsManager.DisableControllerRayInteraction(ControlInputsManager.ControllerSide.RIGHT);
        ModalWindow.SetActive(false);
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneAsync());
    }

    IEnumerator ResetSceneAsync()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        Scene scene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad =  SceneManager.LoadSceneAsync(scene.name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
