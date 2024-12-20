using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabHand : MonoBehaviour
{
    [SerializeField]
    private PXR_Hand _PXRHand;

    [HideInInspector]
    public Transform _attachedObject;

    public XRInteractionManager InteractionManager;

    [HideInInspector]
    public XRIHandInteractorPICO handInteractorPICO;

    public void Start()
    {
        handInteractorPICO = GetComponent<XRIHandInteractorPICO>();
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("OnTriggerStay" + other.name);
        if (_attachedObject == null && other.GetComponent<Rigidbody>() != null && _PXRHand.PinchStrength > 0.7f)
        {
            //Debug.Log("_attachedObject = " + other.transform.name);
            _attachedObject = other.transform; 
        }
        
    }

    public void Update()
    {
        //Debug.Log(_PXRHand.PinchStrength);
        if (_attachedObject != null && _PXRHand.PinchStrength > 0.7f)
        {
            InteractionManager.SelectEnter(handInteractorPICO, _attachedObject.GetComponent<XRGrabInteractable>());
            //Debug.Log("selected object = " + _attachedObject.transform.name);
        }
        else if (_attachedObject != null && _PXRHand.PinchStrength < 0.7f)
        {
            InteractionManager.SelectExit(handInteractorPICO, _attachedObject.GetComponent<XRGrabInteractable>());
            //Debug.Log("released object = " + _attachedObject.transform.name);
            _attachedObject = null;
        }
    }

}
