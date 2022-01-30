using UnityEngine;

public class MoveToAndBack : MonoBehaviour 
{
    public Vector2 endPoint;
    [Range(0, 10)] public float speed = .5f;
    public float startDelay;

    Vector2 startPoint;
    float t;

    void Awake() => startPoint = gameObject.transform.position;

    void Update() 
    {
        if (Time.time <= startDelay)
            return;

        gameObject.transform.position = Vector2.Lerp(startPoint, endPoint, t);
        t += speed * Time.deltaTime;

        if (t >= 1)
        {
            t = 0;
            (startPoint, endPoint) = (endPoint, startPoint);
        }
    }

    void OnDrawGizmos() 
    {
#if UNITY_EDITOR
        Gizmos.color = Color.green;
        Gizmos.DrawLine(gameObject.transform.position, endPoint);
        Gizmos.DrawWireSphere(endPoint, .25f);
#endif
    }
}