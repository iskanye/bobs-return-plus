using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public new CinemachineVirtualCamera camera;

    CinemachineBasicMultiChannelPerlin noise;
    bool isShake;
    float shakeDelay;
    float shakeTime;
    float amplitude;
    float frequency;
    float sharpness = 1;

    void Awake() => noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    void Update()
    {
        if (isShake)
        {
            if (float.IsPositiveInfinity(shakeDelay)) shakeDelay = Time.time + shakeTime;

            noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, frequency, sharpness);
            noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, frequency, sharpness);
            
            if (Time.time >= shakeDelay) isShake = false;
        }

        else
        {
            shakeDelay = float.PositiveInfinity;
            noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, 0, sharpness);
            noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, 0, sharpness);
        }
    }

    public void StartShake(float amplitude, float frequency, float sharpness, float shakeTime)
    {
        isShake = true;
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.sharpness = sharpness;
        this.shakeTime = shakeTime;
    }
}
