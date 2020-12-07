using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera_Scene3a_Roos : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera freeCam;
    [SerializeField] CinemachineVirtualCamera lookAtCam;
    [SerializeField] List<Transform> focusPoints;

    private int focusPointIndex;

    private void Awake()
    {
        SetLookAt();
    }

    public void NextFocusPoint()
    {
        focusPointIndex++;
        SetLookAt();
    }

    private void SetLookAt()
    {

        if(focusPoints[focusPointIndex] == null)
        {
            lookAtCam.enabled = false;
        }

        else
        {
            lookAtCam.enabled = true;
        }

        lookAtCam.LookAt = focusPoints[focusPointIndex];
    }
}
