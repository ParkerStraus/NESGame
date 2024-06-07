using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirCamDollyRig : MonoBehaviour
{
    private CinemachineVirtualCamera virCam;
    private CinemachineTrackedDolly trackedDolly;
    public CinemachineSmoothPath StartingPath;

    // Start is called before the first frame update
    void Start()
    {
        
        virCam = GetComponent<CinemachineVirtualCamera>();
        if (virCam != null)
        {
            trackedDolly = virCam.GetCinemachineComponent<CinemachineTrackedDolly>();
            trackedDolly.m_Path = StartingPath;
        }


    }


    public void NewDolly(CinemachineSmoothPath path)
    {
        if (trackedDolly != null && path != null)
        {
            trackedDolly.m_Path = path;
        }
        else
        {
            Debug.LogWarning("Either trackedDolly or path is null.");
        }
    }
}

