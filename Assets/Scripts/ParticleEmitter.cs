using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.XR.PXR.PXR_Input;
public class ParticleEmitter : MonoBehaviour
{
    public GameObject followed;
    private InputDevice device;
    public Controller controller;
    [SerializeField]
    private ParticleSystem particles;

    [SerializeField]
    private GameObject RightController;
    [SerializeField]
    private GameObject LeftController;

    private void Start()
    {
        InitializeDevice(controller);
    }

    private void InitializeDevice(PXR_Input.Controller side)
    {
        
        device = InputDevices.GetDeviceAtXRNode(controller == PXR_Input.Controller.LeftController ? XRNode.LeftHand : XRNode.RightHand);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, followed.transform.position, Time.deltaTime * 10);

        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
        {

            if (!particles.isPlaying)
            {
                particles.Play();
            }
            if (controller == Controller.RightController)
            {
                SendHapticImpulse(VibrateType.RightController, 0.3f, 100, 150);
                ToolTipManager tpManager = RightController.GetComponentInChildren<ToolTipManager>(true);
                tpManager.DisableTooltip(tpManager.TriggerToolTip);
            }
            if (controller == Controller.LeftController)
            {
                SendHapticImpulse(VibrateType.LeftController, 0.3f, 100, 150);
                ToolTipManager tpManager = LeftController.GetComponentInChildren<ToolTipManager>(true);
                tpManager.DisableTooltip(tpManager.TriggerToolTip);
            }
        }
        else
        {
            if (particles.isPlaying)
            {
                particles.Stop();
            }
        }

    }
}
