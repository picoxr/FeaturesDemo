using System;
using System.Collections;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR;
using static Unity.XR.PXR.PXR_Input;

public class SeethroughWindowManager : MonoBehaviour
{

    private InputDevice Rightdevice;
    private InputDevice Leftdevice;
    public PXR_Input.Controller controller;

    [SerializeField]
    private GameObject windowPrefab;

    [SerializeField]
    private GameObject RightController;
    [SerializeField]
    private GameObject LeftController;

    private GameObject window;

    public static event Action TestedSeethroughEvent;

    private void Start()
    {
        InitializeDevice();
        
    }

    private void OnEnable()
    {
        PXR_Boundary.EnableSeeThroughManual(true);
    }

    // Re-enable seethrough after the app resumes
    void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            PXR_Boundary.EnableSeeThroughManual(true);
        }
    }

    private void InitializeDevice()
    {
        Rightdevice = InputDevices.GetDeviceAtXRNode(controller == PXR_Input.Controller.LeftController ? XRNode.LeftHand : XRNode.RightHand);
        Leftdevice = InputDevices.GetDeviceAtXRNode(controller == PXR_Input.Controller.RightController ? XRNode.LeftHand : XRNode.RightHand);

        InputDevice HeadDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
    }

    // Update is called once per frame
    void Update()
    {
        if (BothControllersGripped() || BothPinched())
        {
            
            if (!window)
            {
                window = Instantiate(windowPrefab);
            }
            else
            {
                SendHapticImpulse(VibrateType.RightController, 0.3f, 100, 130);
                SendHapticImpulse(VibrateType.LeftController, 0.3f, 100, 130);
                ScaleBetweenControllers();
            }
        }
        else
        {
            if (window)
            {
                CheckWindowState();
            } 
        }
    }

   

    private void CheckWindowState()
    {
        if (window.transform.localScale.x < 0.2)
        {
            StartCoroutine(DeleteWindow());
            
            TestedSeethroughEvent?.Invoke();
        }
    }

    private bool BothPinched()
    {
        if(PXR_HandTracking.GetActiveInputDevice() == ActiveInputDevice.HandTrackingActive)
        {
            //if (PXR_HandTracking.GetAimState(0, ref ))
        }
        return false;
    }

    private bool BothControllersGripped()
    {
        if (Rightdevice.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripValue) && rightGripValue)
        {

            if (Leftdevice.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGripValue) && leftGripValue)
            {
                //ToolTipManager tpManager = LeftController.GetComponentInChildren<ToolTipManager>(true);
                //tpManager.DisableTooltip(tpManager.GripToolTip);
                //tpManager = RightController.GetComponentInChildren<ToolTipManager>(true);
                //tpManager.DisableTooltip(tpManager.GripToolTip);
                return true;
            }
        }
        else
        {
            return false;
        }
        return false;
    }

    IEnumerator DeleteWindow()
    {
        float duration = 1f;

        yield return this.AnimateComponent<Transform>(duration, (t, time) =>
        {
            window.transform.localScale = Vector3.Lerp(window.transform.localScale, new Vector3(0f, 0f, 0f), time);
        });

        
        yield return null;
    }

    void ScaleBetweenControllers()
    {
        float factor = 1.0f;

        var dir = LeftController.transform.position - RightController.transform.position;
        var mid = (dir) / 2.0f + RightController.transform.position;
        window.transform.position = mid;
        window.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        Vector3 scale = transform.localScale;
        scale.y = dir.magnitude * factor;
        scale.x = dir.magnitude * factor;
        scale.z = dir.magnitude * factor;
        window.transform.localScale = scale;
    }
}
