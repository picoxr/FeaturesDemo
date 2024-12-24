using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.UI;
using Unity.XR.PXR;

public class HandInteractor : MonoBehaviour
{
    [HideInInspector]
    public bool isGrabbing;

    [HideInInspector]
    public bool isClicking;

    [SerializeField]
    private PXR_Hand _PXRHand;

    [SerializeField]
    private GameObject _rayPointer;

    [SerializeField]
    private Transform _attachedObject;
    private SelectEnterEventArgs args;

    /// <summary>
    /// This method is called every fixed frame-rate frame and is responsible for detecting touch strength and
    /// raycasting objects to see if they can be attached to the hand. If an object is attached, it follows the hand
    /// until it is released.
    /// </summary>
    /// <remarks>
    /// The FixedUpdate() method is called every fixed frame-rate frame. It checks the TouchStrengthRay variable of the
    /// _PXRHand class and raycasts forward to detect any objects that can be grabbed. If an object is detected and the
    /// _attachedObject variable is null, the object is attached to the hand and follows its movements. If the touch
    /// strength is below a certain threshold or the _attachedObject variable is not null, the object is released from the
    /// hand and resumes its natural behavior.
    /// </remarks>
    void FixedUpdate()
    {
        
        if (_PXRHand.PinchStrength > .8f)
        {
            
            Physics.Raycast(_rayPointer.transform.position, transform.forward, out RaycastHit objectHit);

            if (objectHit.rigidbody && _attachedObject == null)
            {
                _attachedObject = objectHit.transform;

                _attachedObject.GetComponent<TargetLazyFollow>().target = transform;
                objectHit.rigidbody.useGravity = false;

                float distance = Vector3.Distance(_attachedObject.position, transform.position);

                _attachedObject.GetComponent<TargetLazyFollow>().targetOffset = new Vector3(0, 0, distance);

                _attachedObject.GetComponent<XRGrabInteractable>().selectEntered.Invoke(args);

            }

            
        }
        else
        {
            if (_attachedObject != null)
            {
                _attachedObject.GetComponent<Rigidbody>().useGravity = true;
                _attachedObject.GetComponent<TargetLazyFollow>().target = null;
                _attachedObject = null;
            }
        }
    }



}
