using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Active { get; private set; }

    new CinemachineVirtualCamera camera;
    CinemachineBasicMultiChannelPerlin noise;
    CinemachineConfiner2D bounds;

    float shakeDelay = float.NegativeInfinity;
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

    public static void StartShake(float amplitude = 18, float frequency = 20, float sharpness = .5f, float shakeTime = .3f)
    {
        var active = Active;

        active.shakeDelay = Time.time + shakeTime;
        active.amplitude = amplitude;
        active.frequency = frequency;
        active.sharpness = sharpness;
    }

    public void ChangeTarget(Transform target) =>
        camera.m_Follow = target;

    public void ChangeTarget(GameObject target) =>
        camera.m_Follow = target.transform;

    public void ChangeCameraBounds(PolygonCollider2D bounds) =>
        this.bounds.m_BoundingShape2D = bounds;
}
