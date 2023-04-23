using UnityEngine;

public class SkullEye : MonoBehaviour
{
    public float radius;

    Vector3 startPos;
    Transform follow;

    void Start() 
    {
        startPos = transform.position;
        follow = FindObjectOfType<PlayerWarp>().player.transform;
    }

    void Update() =>
        transform.position = startPos + radius * (follow.position + (Vector3)Vector2.up - startPos).normalized;
}
