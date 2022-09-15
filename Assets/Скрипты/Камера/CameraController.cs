using UnityEngine.U2D;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Active { get; private set; }

    public PixelPerfectCamera pixelCamera;
    public Transform target;
    public Vector3 offset;

    Vector3 shakeOffset;

    void Awake()
    {
        pixelCamera = GetComponent<PixelPerfectCamera>();
        Active = this;
    }

#if UNITY_ANDROID

    void Start() =>        
        Screen.SetResolution(Screen.width, Screen.height, true);

#endif

    void Update() =>
        transform.position = pixelCamera.RoundToPixel(target.position + offset + shakeOffset);

    public void ChangeTarget(Transform target) =>
        this.target = target;

    public void StartShake(float magnitude, float time)
    {
        StopAllCoroutines();
        StartCoroutine(_StartShake(magnitude, time));
    }

    System.Collections.IEnumerator _StartShake(float magnitude, float time) 
    {
        float _time = 0;

        while (_time < time)
        {
            magnitude = Mathf.Lerp(magnitude, 0, Mathf.Sin(_time / time * Mathf.PI / 2));

            shakeOffset = Random.onUnitSphere * magnitude;
            _time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        shakeOffset = Vector3.zero;
    }
}
