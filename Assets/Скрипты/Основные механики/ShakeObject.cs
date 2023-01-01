using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    [SerializeField] private float magnitude;
    [SerializeField] private float time;

    bool isShaking;

    public void StartShake() =>
        StartShake(magnitude, time);

    public void StartShake(float magnitude) =>
        StartShake(magnitude, time);

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
        var startPos = transform.localPosition;
        isShaking = true;

        while (_time < time)
        {
            magn = Mathf.Lerp(magnitude, 0, _time / time);

            transform.localPosition = startPos + Random.onUnitSphere * magn;
            _time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        isShaking = false;
        transform.localPosition = startPos;
    }
}
