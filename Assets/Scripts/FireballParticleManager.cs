using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.PXR;
using System.Text;
using System;

public class FireballParticleManager : MonoBehaviour
{
    private enum PinchingState
    {
        None,
        Start,
        Pinching,
        End,
    }

    private enum RayClickState
    {
        None,
        Start,
        Clicking,
        End,
    }

    [SerializeField]
    private HandType handSide;

    public PXR_Hand mHand;

    private RayClickState mCurClickState = RayClickState.None;
    private PinchingState mCurPinchingState = PinchingState.None;

    [SerializeField]
    private ParticleSystem particles;

    public AudioSource audiosource_;

    private void OnRayClickDown()
    {
        this.mCurClickState = RayClickState.Clicking;
    }

    private void OnRayClickUp()
    {
        this.mCurClickState = RayClickState.None;
    }

    private void OnPinchingStart()
    {
        mCurPinchingState = PinchingState.Pinching;
    }

    private void OnPinchingEnd()
    {
        mCurPinchingState = PinchingState.None;
    }



    void Start()
    {
        
    }

    private bool mHandTrackingEnable = false;
    private ActiveInputDevice mCurActiveInputDeviceType = ActiveInputDevice.HeadActive;
    private bool handStateQueryResult;
    private HandAimState mHandState = new HandAimState();

    private void UpdateHandTracking(HandType handSide)
    {

        PXR_HandTracking.GetAimState(handSide, ref mHandState);

        if (mHandState.touchStrengthRay > 0.8)
        {
            if (!audiosource_.isPlaying)
            {
                audiosource_.Play();
            }
            
            if (!particles.isPlaying)
            {
                particles.Play();
                
            }
        }
        else
        {
            if (audiosource_.isPlaying)
            {
                audiosource_.Stop();
            }
            if (particles.isPlaying)
            {
                particles.Stop();
                
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHandTracking(handSide);
    }


}
