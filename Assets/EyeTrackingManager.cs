using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR;

public class EyeTrackingManager : MonoBehaviour
{
    public Transform Origin;
    public Transform Greenpoint;

    private Vector3 combineEyeGazeVector;
    private Vector3 combineEyeGazeOriginOffset;
    private Vector3 combineEyeGazeOrigin;
    private Matrix4x4 headPoseMatrix;
    private Matrix4x4 originPoseMatrix;

    private Vector3 combineEyeGazeVectorInWorldSpace;
    private Vector3 combineEyeGazeOriginInWorldSpace;

    private uint leftEyeStatus;
    private uint rightEyeStatus;

    private Vector2 primary2DAxis;

    private RaycastHit hitinfo;

    private Transform selectedObj;

    private bool wasPressed;
    void Start()
    {
        combineEyeGazeOriginOffset = Vector3.zero;
        combineEyeGazeVector = Vector3.zero;
        combineEyeGazeOrigin = Vector3.zero;
        originPoseMatrix = Origin.localToWorldMatrix;
    }

    void Update()
    {
        PXR_EyeTracking.GetHeadPosMatrix(out headPoseMatrix);
        PXR_EyeTracking.GetCombineEyeGazeVector(out combineEyeGazeVector);
        PXR_EyeTracking.GetCombineEyeGazePoint(out combineEyeGazeOrigin);
        //Translate Eye Gaze point and vector to world space

        combineEyeGazeOriginInWorldSpace = originPoseMatrix.MultiplyPoint(headPoseMatrix.MultiplyPoint(combineEyeGazeOrigin));
        combineEyeGazeVectorInWorldSpace = originPoseMatrix.MultiplyVector(headPoseMatrix.MultiplyVector(combineEyeGazeVector));

        GazeTargetControl(combineEyeGazeOriginInWorldSpace, combineEyeGazeVectorInWorldSpace);

    }


    void GazeTargetControl(Vector3 origin, Vector3 vector)
    {
        Ray ray = new Ray(origin, vector);
        if (Physics.SphereCast(origin, 0.0005f, vector, out hitinfo))
        {
            Greenpoint.position = hitinfo.point;
            if (selectedObj != null && selectedObj != hitinfo.transform)
            {
                if (selectedObj.GetComponent<ETObject>() != null)
                    selectedObj.GetComponent<ETObject>().UnFocused();
                selectedObj = null;
            }
            else if (selectedObj == null)
            {
                selectedObj = hitinfo.transform;
                if (selectedObj.GetComponent<ETObject>() != null)
                    
                    selectedObj.GetComponent<ETObject>().IsFocused();
            } 
        }
        else
        {
            if (selectedObj != null)
            {
                if (selectedObj.GetComponent<ETObject>() != null)
                    selectedObj.GetComponent<ETObject>().UnFocused();
                selectedObj = null;
            }
        }
    }
}
