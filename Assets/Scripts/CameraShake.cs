using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cmvc;
    public float shakeIntensity = 1f;
    public float shakeTime = 0.2f;
    private float timer;
    private CinemachineBasicMultiChannelPerlin cbmcp;

    private void Awake()
    {
        cmvc = GetComponent<CinemachineVirtualCamera>();
        cbmcp = cmvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        StopShake();
    }

    public void ShakeCamera()
    {
        cbmcp.m_AmplitudeGain = shakeIntensity;
        timer = shakeTime;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StopShake();
            }
        }
    }

    void StopShake()
    {
        cbmcp.m_AmplitudeGain = 0f;
        timer = 0;
    }
}
