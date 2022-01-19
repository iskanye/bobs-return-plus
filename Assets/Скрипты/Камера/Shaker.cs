using UnityEngine;

public class Shaker : MonoBehaviour
{
    public new CameraShake camera;
    public float amplitude;
    public float frequency;
    [Range(0, 1)] public float sharpness;
    public float shakeTime;

    public void Shake() => camera.StartShake(amplitude, frequency, sharpness, shakeTime);
}
