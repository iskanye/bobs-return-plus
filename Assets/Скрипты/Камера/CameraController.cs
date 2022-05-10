using UnityEngine.U2D;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Active { get; private set; }

    public PixelPerfectCamera pixelCamera;

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

#if UNITY_ANDROID

    void Start() =>        
        Screen.SetResolution(Screen.width, Screen.height, true);

#endif

    void Update()
    {
        noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, Time.time >= shakeDelay ? 0 : frequency, sharpness);
        noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, Time.time >= shakeDelay ? 0 : amplitude, sharpness);

        transform.position = pixelCamera.RoundToPixel(transform.position);
    }

    public static void StartShake(float amplitude = 7, float frequency = 3, float sharpness = .1f, float shakeTime = .6f)
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
