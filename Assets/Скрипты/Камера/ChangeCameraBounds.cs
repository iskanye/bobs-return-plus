using UnityEngine;

public class ChangeCameraBounds : MonoBehaviour
{
    PolygonCollider2D bounds;

    void Awake() =>
        bounds = GetComponent<PolygonCollider2D>();

    public void Change() =>
        CameraController.ChangeCameraBounds(bounds);
}

