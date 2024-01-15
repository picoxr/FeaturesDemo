using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class FaceTrackingManager : MonoBehaviour
{
    public GameObject avatar_1;
    public GameObject avatar_2;

    public SkinnedMeshRenderer skin;
    public SkinnedMeshRenderer tongueBlendShape;
    public SkinnedMeshRenderer leftEyeExample;
    public SkinnedMeshRenderer rightEyeExample;
    public SkinnedMeshRenderer teethBlendShape;

    private UInt64 timestamp;

    private List<string> blendShapeList = new List<string>
    {
"eyeLookDownLeft",
"noseSneerLeft",
"eyeLookInLeft",
"browInnerUp",
"browDownRight",
"mouthClose",
"mouthLowerDownRight",
"jawOpen",
"mouthUpperUpRight",
"mouthShrugUpper",
"mouthFunnel",
"eyeLookInRight",
"eyeLookDownRight",
"noseSneerRight",
"mouthRollUpper",
"jawRight",
"browDownLeft",
"mouthShrugLower",
"mouthRollLower",
"mouthSmileLeft",
"mouthPressLeft",
"mouthSmileRight",
"mouthPressRight",
"mouthDimpleRight",
"mouthLeft",
"jawForward",
"eyeSquintLeft",
"mouthFrownLeft",
"eyeBlinkLeft",
"cheekSquintLeft",
"browOuterUpLeft",
"eyeLookUpLeft",
"jawLeft",
"mouthStretchLeft",
"mouthPucker",
"eyeLookUpRight",
"browOuterUpRight",
"cheekSquintRight",
"eyeBlinkRight",
"mouthUpperUpLeft",
"mouthFrownRight",
"eyeSquintRight",
"mouthStretchRight",
"cheekPuff",
"eyeLookOutLeft",
"eyeLookOutRight",
"eyeWideRight",
"eyeWideLeft",
"mouthRight",
"mouthDimpleLeft",
"mouthLowerDownLeft",
"tongueOut",
    };

    private int[] indexList = new int[52];
    private int tongueIndex;
    private int teethIndex;
    private int leftLookDownIndex;
    private int leftLookUpIndex;
    private int leftLookInIndex;
    private int leftLookOutIndex;

    private int rightLookDownIndex;
    private int rightLookUpIndex;
    private int rightLookInIndex;
    private int rightLookOutIndex;

    /// <summary>
    /// Toggles between two avatar game objects and updates blend shapes.
    /// </summary>
    public void ToggleAvatar()
    {
        if (avatar_1.activeInHierarchy)
        {
            avatar_1.SetActive(false);
            avatar_2.SetActive(true);

            UpdateAvatarBlendShapes(avatar_2);
        }
        else
        {
            avatar_2.SetActive(false);
            avatar_1.SetActive(true);

            UpdateAvatarBlendShapes(avatar_1);
        }
    }

    ///<summary>
    /// Updates the avatar's blend shapes by finding the relevant SkinnedMeshRenderers and calling InitializeBlendShapes().
    /// NOTE: This is hardcoded for Ready Player Me avatars. Other models might not follow this naming convention.
    ///</summary>
    /// <param name="avatar">The GameObject of the avatar whose blend shapes will be updated.</param>
    private void UpdateAvatarBlendShapes(GameObject avatar)
    {
        skin = avatar.transform.Find("Wolf3D_Head").GetComponent<SkinnedMeshRenderer>();
        tongueBlendShape = avatar.transform.Find("Wolf3D_Teeth").GetComponent<SkinnedMeshRenderer>();
        rightEyeExample = avatar.transform.Find("EyeRight").GetComponent<SkinnedMeshRenderer>();
        leftEyeExample = avatar.transform.Find("EyeLeft").GetComponent<SkinnedMeshRenderer>();
        teethBlendShape = avatar.transform.Find("Wolf3D_Teeth").GetComponent<SkinnedMeshRenderer>();

        InitializeBlendShapes();
    }


    TrackingStateCode trackingState;

    // Start is called before the first frame update
    void Start()
    {
        // Want face tracking for the current app

        trackingState = (TrackingStateCode)PXR_MotionTracking.WantFaceTrackingService();

        // Query if the current device support face tracking
        bool supported = false;
        int supportedCount = 0;
        FaceTrackingSupportedMode faceTrackingMode = FaceTrackingSupportedMode.PXR_FTM_FACE_LIPS_BS;
        trackingState = (TrackingStateCode)PXR_MotionTracking.GetFaceTrackingSupported(ref supported, ref supportedCount, ref faceTrackingMode);

        // Start face tracking
        FaceTrackingStartInfo info = new FaceTrackingStartInfo();
        info.mode = FaceTrackingSupportedMode.PXR_FTM_FACE;
        trackingState = (TrackingStateCode)PXR_MotionTracking.StartFaceTracking(ref info);


        InitializeBlendShapes();
    }

    private void InitializeBlendShapes()
    {
        for (int i = 0; i < indexList.Length; i++)
        {
            indexList[i] = skin.sharedMesh.GetBlendShapeIndex(blendShapeList[i]);
        }

        tongueIndex = tongueBlendShape.sharedMesh.GetBlendShapeIndex("tongueOut");
        teethIndex = teethBlendShape.sharedMesh.GetBlendShapeIndex("jawOpen");
        leftLookDownIndex = leftEyeExample.sharedMesh.GetBlendShapeIndex("eyeLookDownLeft");
        leftLookUpIndex = leftEyeExample.sharedMesh.GetBlendShapeIndex("eyeLookUpLeft");
        leftLookInIndex = leftEyeExample.sharedMesh.GetBlendShapeIndex("eyeLookInLeft");
        leftLookOutIndex = leftEyeExample.sharedMesh.GetBlendShapeIndex("eyeLookOutLeft");
        rightLookDownIndex = rightEyeExample.sharedMesh.GetBlendShapeIndex("eyeLookDownRight");
        rightLookUpIndex = rightEyeExample.sharedMesh.GetBlendShapeIndex("eyeLookUpRight");
        rightLookInIndex = rightEyeExample.sharedMesh.GetBlendShapeIndex("eyeLookInRight");
        rightLookOutIndex = rightEyeExample.sharedMesh.GetBlendShapeIndex("eyeLookOutRight");
    }

    FaceTrackingDataGetInfo faceInfo = new FaceTrackingDataGetInfo();

    // Update is called once per frame
    unsafe void Update()
    {

        FaceTrackingDataGetInfo info = new FaceTrackingDataGetInfo();
        info.displayTime = 0;
        info.flags = FaceTrackingDataGetFlags.PXR_FACE_DEFAULT;
        FaceTrackingData faceTrackingData = new FaceTrackingData();
        float* b = stackalloc float[72]; // The array's length must by 72, otherwise the request will return an error
        faceTrackingData.blendShapeWeight = b;
        trackingState = (TrackingStateCode)PXR_MotionTracking.GetFaceTrackingData(ref info, ref faceTrackingData);

        tongueBlendShape.SetBlendShapeWeight(tongueIndex, faceTrackingData.blendShapeWeight[51]);
        teethBlendShape.SetBlendShapeWeight(teethIndex, faceTrackingData.blendShapeWeight[8]);

        leftEyeExample.SetBlendShapeWeight(leftLookUpIndex, faceTrackingData.blendShapeWeight[31]);
        leftEyeExample.SetBlendShapeWeight(leftLookDownIndex, faceTrackingData.blendShapeWeight[0]);
        leftEyeExample.SetBlendShapeWeight(leftLookInIndex, faceTrackingData.blendShapeWeight[2]);
        leftEyeExample.SetBlendShapeWeight(leftLookOutIndex, faceTrackingData.blendShapeWeight[44]);

        rightEyeExample.SetBlendShapeWeight(rightLookUpIndex, faceTrackingData.blendShapeWeight[35]);
        rightEyeExample.SetBlendShapeWeight(rightLookDownIndex, faceTrackingData.blendShapeWeight[12]);
        rightEyeExample.SetBlendShapeWeight(rightLookInIndex, faceTrackingData.blendShapeWeight[11]);
        rightEyeExample.SetBlendShapeWeight(rightLookOutIndex, faceTrackingData.blendShapeWeight[45]);


        for (int i = 0; i < indexList.Length; ++i)
        {
            if (indexList[i] >= 0)
            {
                skin.SetBlendShapeWeight(indexList[i], faceTrackingData.blendShapeWeight[i]);
                //Debug.Log("Setting " + blendShapeList[i] + "[" + indexList[i] + "] with value " + faceTrackingData.blendShapeWeight[i]);
            }
        }
    }
}
