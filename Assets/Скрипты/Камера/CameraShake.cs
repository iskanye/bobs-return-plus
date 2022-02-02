using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public new CinemachineVirtualCamera camera;

    CinemachineBasicMultiChannelPerlin noise;
    float shakeDelay = float.NegativeInfinity;
    float shakeTime;
    float amplitude;
    float frequency;
    float sharpness = 1;

    void Awake() => noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    void Update()
    {
        noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, Time.time >= shakeDelay ? 0 : frequency, sharpness);
        noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, Time.time >= shakeDelay ? 0 : amplitude, sharpness);
    }

    public void StartShake(float amplitude = 10, float frequency = 10, float sharpness = .1f, float shakeTime = .4f)
    {
        shakeDelay = Time.time + shakeTime;
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.sharpness = sharpness;
        this.shakeTime = shakeTime;
    }
}
