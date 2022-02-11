using UnityEngine;

public class Shaker : MonoBehaviour
{
    public float amplitude;
    public float frequency;
    [Range(0, 1)] public float sharpness;
    public float shakeTime;

    public void Shake() => CameraController.StartShake(amplitude, frequency, sharpness, shakeTime);
}
