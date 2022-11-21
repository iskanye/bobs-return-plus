using UnityEngine.U2D;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Active { get; private set; }

    public ShakeObject shake;
    public PixelPerfectCamera pixelCamera;
    public Transform target;
    public Vector3 offset;


    void Awake() =>
        Active = this;

#if UNITY_ANDROID

    void Start() =>        
        Screen.SetResolution(Screen.width, Screen.height, true);

#endif

    void Update() =>
        transform.position = pixelCamera.RoundToPixel(target.position + offset);

    public void ChangeTarget(Transform target) =>
        this.target = target;

    public void StartShake(float magnitude, float time) =>
        shake.StartShake(magnitude, time);
}
