using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public new CinemachineVirtualCamera camera;

    public static CameraShake Active { get; private set; }

    CinemachineBasicMultiChannelPerlin noise;
    float shakeDelay = float.NegativeInfinity;
    float shakeTime;
    float amplitude;
    float frequency;
    float sharpness = 1;

    void Awake() 
    { 
        noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Active = this;
    }

    void Update()
    {
        noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, Time.time >= shakeDelay ? 0 : frequency, sharpness);
        noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, Time.time >= shakeDelay ? 0 : amplitude, sharpness);
    }

    public static void StartShake(float amplitude = 10, float frequency = 10, float sharpness = .1f, float shakeTime = .4f)
    {
        var active = Active;

        active.shakeDelay = Time.time + shakeTime;
        active.amplitude = amplitude;
        active.frequency = frequency;
        active.sharpness = sharpness;
        active.shakeTime = shakeTime;
    }
}
