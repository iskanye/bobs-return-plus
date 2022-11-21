using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    [SerializeField] private float magnitude;
    [SerializeField] private float time;

    public void StartShake() =>
        StartShake(magnitude, time);

    public void StartShake(float magnitude) =>
        StartShake(magnitude, time);

    public void StartShake(float magnitude, float time)
    {
        StopAllCoroutines();
        StartCoroutine(_StartShake(magnitude, time));
    }

    System.Collections.IEnumerator _StartShake(float magnitude, float time) 
    {
        float _time = 0;
        var startPos = transform.position;

        while (_time < time)
        {
            magnitude = Mathf.Lerp(magnitude, 0, Mathf.Sin(_time / time * Mathf.PI / 2));

            transform.position = startPos + Random.onUnitSphere * magnitude;
            _time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.position = startPos;
    }
}
