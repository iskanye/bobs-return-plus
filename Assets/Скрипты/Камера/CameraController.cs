using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Active { get; private set; }

    new CinemachineVirtualCamera camera;
    CinemachineBasicMultiChannelPerlin noise;
    CinemachineConfiner2D bounds;

    float shakeDelay = float.NegativeInfinity;
    float shakeTime;
    float amplitude;
    float frequency;
    float sharpness = 1;

    void Awake() 
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        bounds = GetComponent<CinemachineConfiner2D>();

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

    public static void ChangeTarget(Transform target) =>
        Active.camera.m_Follow = target;

    public static void ChangeCameraBounds(PolygonCollider2D bounds) =>
        Active.bounds.m_BoundingShape2D = bounds;
}
