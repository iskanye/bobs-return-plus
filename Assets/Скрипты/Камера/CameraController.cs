using UnityEngine.U2D;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Active { get; private set; }

    public CinemachineVirtualCamera cinemachineCamera;
    public PixelPerfectCamera pixelCamera;

    CinemachineFramingTransposer transposer;
    bool isShaking;

    void Awake() =>
        Active = this;

    void Start() 
    {   
#if UNITY_ANDROID    
        Screen.SetResolution(Screen.width, Screen.height, true);
#endif
        transposer = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void FixedUpdate() =>
        transform.position = pixelCamera.RoundToPixel(Vector3.zero);

    public void ChangeTarget(Transform target) =>
        cinemachineCamera.Follow = target;

    public void StartShake(float magnitude, float time)
    {
        if (isShaking)
            return;

        StopAllCoroutines();
        StartCoroutine(_StartShake(magnitude, time));
    }

    System.Collections.IEnumerator _StartShake(float magnitude, float time) 
    {
        float _time = 0, magn;
        var startPos = transposer.m_TrackedObjectOffset;
        isShaking = true;

        while (_time < time)
        {
            magn = Mathf.Lerp(magnitude, 0, _time / time);

            transposer.m_TrackedObjectOffset = startPos + Random.onUnitSphere * magn;
            _time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        isShaking = false;
        transposer.m_TrackedObjectOffset = startPos;
    }
}
